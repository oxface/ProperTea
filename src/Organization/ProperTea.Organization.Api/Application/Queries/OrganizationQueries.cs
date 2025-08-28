using ProperTea.Contracts.CQRS;
using ProperTea.Contracts.DTOs.Organization;

namespace ProperTea.Organization.Api.Application.Queries;

public record GetOrganizationByIdQuery(Guid OrganizationId) : IQuery<OrganizationDto?>;

public record GetOrganizationByNameQuery(string Name) : IQuery<OrganizationDto?>;

public record CheckOrganizationExistsQuery(Guid OrganizationId) : IQuery<bool>;
