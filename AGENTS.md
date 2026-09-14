# AGENTS.md

## Repository purpose

Physical source-code layout:

```text
src/
├── App/
│   ├── Framework/
│   └── Modules/
│       ├── Module1/
│       └── Module2/
├── Tests/
└── Tools/
```

Place new projects in this layout: shared technical code goes in `App/Framework`, code belonging to a specific module goes in `App/Modules/<ModuleName>`, tests go in `Tests`, and console or supporting tools go in `Tools`.

`AGENTS.md` contains only the context and rules relevant to changes in this repository. Verify implementation details against the existing code, projects, and architecture tests.

## Common commands

Run commands from the `src` directory:

```bash
dotnet restore ModularMonolith.sln
dotnet build ModularMonolith.sln
dotnet test ModularMonolith.sln
dotnet run --project App/ModularMonolith.Web
```

Use the dedicated tool for migrations instead of running `dotnet ef` in an arbitrary order:

```bash
dotnet run --project Tools/ModularMonolith.Tools.Migrator -- --update
dotnet run --project Tools/ModularMonolith.Tools.Migrator -- --sql
```

`--update` applies migrations in dependency order. `--sql` creates idempotent SQL scripts in `src/Tools/ModularMonolith.Tools.Migrator/Scripts/Sql`.

## Modules

A module is an isolated vertical part of the domain with its own public contract. The current solution contains `Template` and `Builder` modules; their project names start with `ModularMonolith.Module.<ModuleName>`.

A typical module consists of:

- `*.Domain` - aggregates, entities, and domain rules.
- `*.Contract` - the module public surface for other modules: contract interfaces and DTOs for inter-module communication.
- `*.InputPorts.UseCases` - application-layer input ports: use case interfaces, commands, queries, and DTOs used by primary adapters.
- `*.Contract.Implementation` - contract implementation and the `Module` that composes the module dependencies.
- `*.UseCases` - MediatR handlers and application scenarios.
- `*.Controllers` and `*.BackgroundJobs` - primary adapters that invoke use cases.
- `*.DataAccess.Interface` - the output port for data access.
- `*.DataAccess.<dbname>` - the EF Core adapter and migrations.

Module boundary rules:

- Access another module only through its `*.Contract`. Do not reference its `Domain`, `UseCases`, `Controllers`, `DataAccess.*`, or `Contract.Implementation`.
- Do not move entities between modules or add navigation properties to entities from another module. Store an identifier for a relationship, for example `Builder.TemplateId`.
- Each module owns its model, `DbContext`, database schema, and migrations.
- Referential integrity may exist at the database level, but code dependencies between module models are prohibited.
- Do not create cyclic dependencies between modules. When adding a module or dependency, update the list and order in `Tools/ModularMonolith.Tools.Migrator/Program.cs`: migrations must run from independent modules to dependent ones.
- When a scenario spans multiple modules, begin the transaction in the orchestrating module and attach the other contracts through `IUnitOfWork`. Do not replace this with direct access to another module `DbContext`.

## Layers within a module

The project uses hexagonal/clean architecture:

- Primary adapters (`Controllers`, `BackgroundJobs`) receive HTTP requests or Quartz events and pass DTOs, commands, or queries through `*.InputPorts.UseCases` to the application layer.
- `Contract` is the input port for another module; `UseCases` orchestrate user scenarios through MediatR.
- `DataAccess.Interface` is the output port; `DataAccess.<dbname>` is its implementation.
- `Domain` does not depend on Web, EF Core, or another module.

Controllers must not work directly with `DbContext`, EF entities, or external adapters. Do not pass HTTP models into the domain: use DTOs from `*.Contract`. Do not bypass a use case just to call an adapter. The exception is a module contract when another module invokes it across an explicit boundary.

## Framework

`ModularMonolith.Framework.*` is shared technical infrastructure reused by several modules, not a separate business module. It contains common abstractions and implementations:

- `Framework.Entities` - base `Entity`, `Aggregate`, and auditing types.
- `Framework.DataAccess` - shared `IDbContext`, `IContract`, and `IUnitOfWork` interfaces, plus transaction coordination.
- `Framework.UseCases.Interfaces` - common exceptions and application-layer contracts.
- `Framework.UseCases.Implementation` - shared DI registration, including `ModularEfUnitOfWork`.
- `Framework.Utils` - infrastructure utilities, including the base `Module` and `RegisterModule`.

Put code in `Framework` only when it is genuinely domain-neutral and needed by more than one module. Do not add DTOs, use cases, entities, or dependencies of a specific module there. A module may depend on Framework; Framework must not depend on a module.

## Web as the composition root

`App/ModularMonolith.Web` is the only HTTP host and the composition root, not a business module. In `Program.cs`, it:

- configures common ASP.NET Core services, OpenAPI/Swagger, middleware, and routing;
- registers `FrameworkModule` and then the modules through `RegisterModule<T>(configuration)`;
- loads module controllers: each module does this in its own `Module.Load` implementation through `AddApplicationPart`.

When adding a module, create its `Module` implementation, register its services, adapters, MediatR handlers, controllers, and jobs there, then explicitly register that `Module` in `App/ModularMonolith.Web/Program.cs`. Do not place module business logic, EF configuration, or direct calls to module adapters in Web.

## Changes and verification

- Follow the existing C# configuration: nullable reference types are enabled, and use file-scoped namespaces and primary constructors where they are already appropriate.
- When changing architectural boundaries, update `Tests/ModularMonolith.Tests.Unit/ArchitectureTests.cs`; keep layer and module reference checks current.
- After changing a schema, create a migration in the corresponding `*.DataAccess.<dbname>` project and verify it through the migrator.
- Do not change auto-generated EF migration files unless necessary.
- Before completing a change, run the narrowest relevant verification and, when appropriate, `dotnet build ModularMonolith.sln` and `dotnet test ModularMonolith.sln`.
