using ProperTea.Cqrs;

namespace ProperTea.Organization.Api.Application.Queries;

public record GetOrganizationByIdQuery(Guid OrganizationId) : IQuery;

public record GetOrganizationByNameQuery(string Name) : IQuery;

public record CheckOrganizationExistsQuery(Guid OrganizationId) : IQuery;