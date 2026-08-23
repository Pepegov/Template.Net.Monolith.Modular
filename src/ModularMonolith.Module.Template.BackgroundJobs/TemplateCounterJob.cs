using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModularMonolith.Module.Template.DataAccess.Interface;
using Quartz;

namespace ModularMonolith.Module.Template.BackgroundJobs;

public class TemplateCounterJob(ILogger<TemplateCounterJob> logger, ITemplateDbContext templateDbContext) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var value = await templateDbContext.Templates.CountAsync(context.CancellationToken);
        logger.LogInformation($"Template count: {value}");
    }
}