using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Module.Builder.BackgroundJobs;
using ModularMonolith.Module.Builder.Controllers;
using ModularMonolith.Module.Builder.DataAccess.Interface;
using ModularMonolith.Module.Builder.DataAccess.Sqlite;
using ModularMonolith.Module.Builder.UseCases.Builder.Handles;
using Quartz;

namespace ModularMonolith.Module.Builder.Contracts.Implementation;

public class BuilderModule : Framework.Utils.Modules.Module
{
    public override void Load(IServiceCollection services, IConfiguration configuration)
    {
        // Background jobs
        services.AddQuartz(q =>
        {
            q.AddJob<BuilderCounterJob>(opts => opts
                .WithIdentity("builder-counter-job")
                .StoreDurably());
            q.AddTrigger(t => t
                .WithIdentity("builder-counter-trigger")
                .ForJob("builder-counter-job")
                .WithCronSchedule("0 */1 * * * ?"));
        });

        // Contract
        services.AddScoped<IBuilderContract, BuilderContract>();
        
        // DataAccess
        var builderConnectionString = configuration.GetConnectionString("Sqlite") ?? "Data Source=modular.db";
        services.AddDbContext<BuilderDbContext>((sp, bld) =>
        {
            bld.UseSqlite(builderConnectionString,
                opt => opt.MigrationsHistoryTable(HistoryRepository.DefaultTableName, BuilderDbContext.Schema));
        });
        
        services.AddScoped<IBuilderDbContext, BuilderDbContext>();
        
        // UseCases
        services.AddMediatR(configuration => 
            configuration.RegisterServicesFromAssembly(typeof(BuilderCreateCommandHandler).Assembly));
        
        services.AddControllers().AddApplicationPart(typeof(BuilderController).Assembly);
    }
}
