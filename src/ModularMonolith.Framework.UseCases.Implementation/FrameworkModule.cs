using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Framework.DataAccess.UnitOfWork;
using ModularMonolith.Framework.Utils.Modules;

namespace ModularMonolith.Framework.UseCases.Implementation;

public class FrameworkModule : Module
{
    public override void Load(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, ModularEfUnitOfWork>();
    }
}