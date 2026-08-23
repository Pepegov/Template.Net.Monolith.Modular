using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModularMonolith.Module.Builder.DataAccess.Interface;
using Quartz;

namespace ModularMonolith.Module.Builder.BackgroundJobs;

public class BuilderCounterJob(ILogger<BuilderCounterJob> logger, IBuilderDbContext builderDbContext) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var value = await builderDbContext.Builders.CountAsync(context.CancellationToken);
        logger.LogInformation($"Builder count: {value}");
    }
}
