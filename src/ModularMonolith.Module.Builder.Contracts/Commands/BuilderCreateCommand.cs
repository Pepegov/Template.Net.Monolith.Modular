using MediatR;
using ModularMonolith.Module.Builder.Contracts.Dto;

namespace ModularMonolith.Module.Builder.Contracts.Commands;

public record BuilderCreateCommand(BuilderDto Model) : IRequest<Unit>;
