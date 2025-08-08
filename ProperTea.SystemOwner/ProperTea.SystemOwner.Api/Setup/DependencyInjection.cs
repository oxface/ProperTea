using FluentValidation;

using ProperTea.Shared.Application;
using ProperTea.Shared.Application.Commands;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain;
using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.Shared.Infrastructure.Data.Ef;
using ProperTea.Shared.Infrastructure.Events;
using ProperTea.SystemOwner.Application.SystemOwner.Commands;
using ProperTea.SystemOwner.Application.SystemOwner.DomainEventHandlers;
using ProperTea.SystemOwner.Application.SystemOwner.Queries;
using ProperTea.SystemOwner.Domain.SystemOwner;
using ProperTea.SystemOwner.Infrastructure.SystemOwner.Data;

namespace ProperTea.SystemOwner.Api.Setup;

public static class DomainServices
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ISystemOwnerDomainService, SystemOwnerDomainService>();

        return services;
    }
}

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ChangeSystemOwnerNameCommandHandler).Assembly);

        services.Scan(scan => scan
            .FromAssemblyOf<ChangeSystemOwnerNameCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<ChangeSystemOwnerNameCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<GetSystemOwnerByIdQueryHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));

        services.Scan(scan => scan
            .FromAssemblyOf<SystemOwnerCreatedDomainEventHandler>()
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
            .FromAssemblyOf<SystemOwnerRepository>()
            .AddClasses(classes => classes.AssignableTo(typeof(IRepository<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}