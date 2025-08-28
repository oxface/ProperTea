using ProperTea.Contracts.CQRS;
using ProperTea.Contracts.DTOs.Organization;
using ProperTea.Organization.Api.Application.Queries;
using ProperTea.Organization.Api.Domain.Organizations;

namespace ProperTea.Organization.Api.Application.Handlers;

public class GetOrganizationByNameQueryHandler : IQueryHandler<GetOrganizationByNameQuery, OrganizationDto?>
{
    private readonly IOrganizationRepository _organizationRepository;

    public GetOrganizationByNameQueryHandler(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<OrganizationDto?> HandleAsync(GetOrganizationByNameQuery query, CancellationToken cancellationToken = default)
    {
        var organization = await _organizationRepository.GetByNameAsync(query.Name, cancellationToken);
        
        return organization == null ? null : new OrganizationDto(
            organization.Id,
            organization.Name,
            organization.Description,
            organization.CreatedAt,
            organization.IsActive);
    }
}
