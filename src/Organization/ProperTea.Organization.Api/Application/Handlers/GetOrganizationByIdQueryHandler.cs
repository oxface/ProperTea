using ProperTea.Cqrs;
using ProperTea.Organization.Api.Application.Models;
using ProperTea.Organization.Api.Application.Queries;
using ProperTea.Organization.Api.Domain.Organizations;

namespace ProperTea.Organization.Api.Application.Handlers;

public class GetOrganizationByIdQueryHandler(IOrganizationRepository organizationRepository)
    : IQueryHandler<GetOrganizationByIdQuery, OrganizationModel?>
{
    public async Task<OrganizationModel?> HandleAsync(GetOrganizationByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var organization = await organizationRepository.GetByIdAsync(query.OrganizationId, cancellationToken);
        return organization == null ? null : OrganizationModel.FromEntity(organization);
    }
}