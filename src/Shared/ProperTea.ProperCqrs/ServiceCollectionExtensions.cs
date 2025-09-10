using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ProperTea.Cqrs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProperCqrs(this IServiceCollection services)
    {
        services.AddSingleton<ICommandBus, CommandBus>();
        services.AddSingleton<IQueryBus, QueryBus>();

        return services;
    }
}