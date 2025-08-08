using FluentValidation;

using ProperTea.Organization.Application.Commands;
using ProperTea.Organization.Application.DomainEventHandlers;
using ProperTea.Organization.Application.Queries;
using ProperTea.Organization.Domain;
using ProperTea.Organization.Infrastructure.Data;
using ProperTea.Shared.Application;
using ProperTea.Shared.Application.Commands;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain;
using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.Shared.Infrastructure.Data.Ef;
using ProperTea.Shared.Infrastructure.Events;

namespace ProperTea.Organization.Api.Setup;

public static class DomainServices
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationDomainService, OrganizationDomainService>();

        return services;
    }
}

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ChangeOrganizationNameCommandHandler).Assembly);

        services.Scan(scan => scan
            .FromAssemblyOf<ChangeOrganizationNameCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<ChangeOrganizationNameCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<GetOrganizationByIdQueryHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));

        services.Scan(scan => scan
            .FromAssemblyOf<OrganizationCreatedDomainEventHandler>()
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
            .FromAssemblyOf<OrganizationRepository>()
            .AddClasses(classes => classes.AssignableTo(typeof(IRepository<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}