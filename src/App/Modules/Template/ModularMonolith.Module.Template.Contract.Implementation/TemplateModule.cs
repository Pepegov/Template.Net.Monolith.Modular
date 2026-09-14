using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Module.Template.BackgroundJobs;
using ModularMonolith.Module.Template.Controllers;
using ModularMonolith.Module.Template.DataAccess.Interface;
using ModularMonolith.Module.Template.DataAccess.Sqlite;
using ModularMonolith.Module.Template.UseCases.Template.Handles;
using Quartz;

namespace ModularMonolith.Module.Template.Contract.Implementation;

public class TemplateModule : Framework.Utils.Modules.Module
{
    public override void Load(IServiceCollection services, IConfiguration configuration)
    {
        // Background jobs
        services.AddQuartz(q =>
        {
            q.AddJob<TemplateCounterJob>(opts => opts
                .WithIdentity("template-counter-job")
                .StoreDurably());
            q.AddTrigger(t => t
                .WithIdentity("template-counter-trigger")
                .ForJob("template-counter-job")
                .WithCronSchedule("0 */1 * * * ?"));
        });

        // Contract
        services.AddScoped<ITemplateContract, TemplateContract>();
        
        // DataAccess
        var templateConnectionString = configuration.GetConnectionString("Sqlite") ?? "Data Source=modular.db";
        services.AddDbContext<TemplateDbContext>((sp, bld) =>
        {
            bld.UseSqlite(templateConnectionString,
                opt => opt.MigrationsHistoryTable(HistoryRepository.DefaultTableName, TemplateDbContext.Schema));
        });
        
        services.AddScoped<ITemplateDbContext, TemplateDbContext>();
        
        // UseCases
        services.AddMediatR(configuration => 
            configuration.RegisterServicesFromAssembly(typeof(TemplateCreateCommandHandler).Assembly));
        
        services.AddControllers().AddApplicationPart(typeof(TemplateController).Assembly);
    }
}