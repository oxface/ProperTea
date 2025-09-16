using FluentValidation;
using ProperTea.ProperCqrs;
using ProperTea.Shared.Application;
using ProperTea.Shared.Domain;
using ProperTea.Shared.Domain.Events;
using ProperTea.Shared.Infrastructure.Events;
using ProperTea.Shared.Infrastructure.Persistence;
using ProperTea.UserManagement.Application.Users.Commands;
using ProperTea.UserManagement.Domain.Users;
using ProperTea.UserManagement.Infrastructure.Persistence;

namespace ProperTea.UserManagement.Api.Configuration;

public static class DomainServices
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<User>()
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
        services.AddValidatorsFromAssembly(typeof(CreateUserCommandHandler).Assembly);

        services.Scan(scan => scan
            .FromAssemblyOf<CreateUserCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<CreateUserCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<CreateUserCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));

        services.Scan(scan => scan
            .FromAssemblyOf<UserCreatedDomainEvent>()
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
            .FromAssemblyOf<UserRepository>()
            .AddClasses(classes => classes.AssignableTo(typeof(IRepository<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}