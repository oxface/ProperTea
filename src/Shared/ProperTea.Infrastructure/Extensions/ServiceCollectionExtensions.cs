using Microsoft.Extensions.DependencyInjection;
using ProperTea.Contracts.CQRS;
using ProperTea.Infrastructure.CQRS;

namespace ProperTea.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomCqrs(this IServiceCollection services)
    {
        services.AddSingleton<ICommandBus, CommandBus>();
        services.AddSingleton<IQueryBus, QueryBus>();
        
        return services;
    }

    public static IServiceCollection AddCommandHandlers(this IServiceCollection services, params Type[] assemblyMarkerTypes)
    {
        foreach (var markerType in assemblyMarkerTypes)
        {
            var assembly = markerType.Assembly;
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                var interfaces = handlerType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

                foreach (var @interface in interfaces)
                {
                    services.AddScoped(@interface, handlerType);
                }
            }
        }

        return services;
    }

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services, params Type[] assemblyMarkerTypes)
    {
        foreach (var markerType in assemblyMarkerTypes)
        {
            var assembly = markerType.Assembly;
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                var interfaces = handlerType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

                foreach (var @interface in interfaces)
                {
                    services.AddScoped(@interface, handlerType);
                }
            }
        }

        return services;
    }
}
