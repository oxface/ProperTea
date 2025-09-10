using FluentValidation;
using ProperTea.Application.Shared;
using ProperTea.Cqrs;
using ProperTea.Domain.Shared;
using ProperTea.Domain.Shared.Events;
using ProperTea.Infrastructure.Shared.Events;
using ProperTea.Infrastructure.Shared.Persistence;
using ProperTea.UserManagement.Application.Commands;
using ProperTea.UserManagement.Domain.SystemUsers;
using ProperTea.UserManagement.Infrastructure.Persistence;

namespace ProperTea.UserManagement.Api.Configuration;

public static class DomainServices
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<SystemUser>()
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainService)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CreateSystemUserCommandHandler).Assembly);

        services.Scan(scan => scan
            .FromAssemblyOf<CreateSystemUserCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<CreateSystemUserCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<CreateSystemUserCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));

        services.Scan(scan => scan
            .FromAssemblyOf<SystemUserCreatedDomainEvent>()
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<SystemUserRepository>()
            .AddClasses(classes => classes.AssignableTo(typeof(IRepository<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}