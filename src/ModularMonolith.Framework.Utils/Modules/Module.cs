using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Framework.Utils.Modules;

public abstract class Module 
{
    public abstract void Load(IServiceCollection services, IConfiguration configuration);
}