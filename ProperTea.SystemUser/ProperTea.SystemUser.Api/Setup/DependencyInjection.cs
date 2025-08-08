using FluentValidation;

using ProperTea.Shared.Application;
using ProperTea.Shared.Application.Commands;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain;
using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.Shared.Infrastructure.Data.Ef;
using ProperTea.Shared.Infrastructure.Events;
using ProperTea.SystemUser.Application.Commands;
using ProperTea.SystemUser.Application.DomainEventHandlers;
using ProperTea.SystemUser.Application.Queries;
using ProperTea.SystemUser.Domain;
using ProperTea.SystemUser.Infrastructure.Data;

namespace ProperTea.SystemUser.Api.Setup;

public static class DomainServices
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ISystemUserDomainService, SystemUserDomainService>();

        return services;
    }
}

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ChangeSystemUserNameCommandHandler).Assembly);

        services.Scan(scan => scan
            .FromAssemblyOf<ChangeSystemUserNameCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<ChangeSystemUserNameCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<GetSystemUserByIdQueryHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));

        services.Scan(scan => scan
            .FromAssemblyOf<SystemUserCreatedDomainEventHandler>()
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