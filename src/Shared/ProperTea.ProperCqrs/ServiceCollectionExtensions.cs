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

    public static IServiceCollection AddProperCqrsCommandHandlers(this IServiceCollection services,
        Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

            foreach (var interfaceType in interfaces) services.AddTransient(interfaceType, handlerType);
        }

        return services;
    }

    public static IServiceCollection AddProperCqrsQueryHandlers(this IServiceCollection services,
        Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

            foreach (var interfaceType in interfaces) services.AddTransient(interfaceType, handlerType);
        }

        return services;
    }
}