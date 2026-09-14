using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Framework.Utils.Modules;

public static class ServiceCollectionExtensions
{
    public static void RegisterModule<T>(this IServiceCollection serviceCollection, IConfiguration configuration) where T : Module
    {
        var module = Activator.CreateInstance<T>();

        module.Load(serviceCollection, configuration);
    }
}