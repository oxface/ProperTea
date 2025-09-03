using ProperTea.Contracts.CQRS;
using ProperTea.Organization.Api.Application.Models;

namespace ProperTea.Organization.Api.Application.Queries;

public record GetOrganizationByIdQuery(Guid OrganizationId) : IQuery<OrganizationModel?>;

public record GetOrganizationByNameQuery(string Name) : IQuery<OrganizationModel?>;

public record CheckOrganizationExistsQuery(Guid OrganizationId) : IQuery<bool>;
