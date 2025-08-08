using FluentValidation;

using ProperTea.Company.Application.Company.Commands;
using ProperTea.Company.Application.Company.DomainEventHandlers;
using ProperTea.Company.Application.Company.Queries;
using ProperTea.Company.Domain.Company;
using ProperTea.Company.Infrastructure.Company.Data;
using ProperTea.Shared.Application;
using ProperTea.Shared.Application.Commands;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain;
using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.Shared.Infrastructure.Data.Ef;
using ProperTea.Shared.Infrastructure.Events;

namespace ProperTea.Company.Api.Setup;

public static class DomainServices
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ICompanyDomainService, CompanyDomainService>();

        return services;
    }
}

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ChangeCompanyNameCommandHandler).Assembly);

        services.Scan(scan => scan
            .FromAssemblyOf<ChangeCompanyNameCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<ChangeCompanyNameCommandHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<GetCompanyByIdQueryHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));

        services.Scan(scan => scan
            .FromAssemblyOf<CompanyCreatedDomainEventHandler>()
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
            .FromAssemblyOf<CompanyRepository>()
            .AddClasses(classes => classes.AssignableTo(typeof(IRepository<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}