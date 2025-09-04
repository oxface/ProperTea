using ProperTea.Cqrs;
using ProperTea.Organization.Api.Application.Models;
using ProperTea.Organization.Api.Application.Queries;
using ProperTea.Organization.Api.Domain.Organizations;

namespace ProperTea.Organization.Api.Application.Handlers;

public class GetOrganizationByNameQueryHandler(IOrganizationRepository organizationRepository)
    : IQueryHandler<GetOrganizationByNameQuery, OrganizationModel?>
{
    public async Task<OrganizationModel?> HandleAsync(GetOrganizationByNameQuery query,
        CancellationToken cancellationToken = default)
    {
        var organization = await organizationRepository.GetByNameAsync(query.Name, cancellationToken);

        return organization == null ? null : OrganizationModel.FromEntity(organization);
    }
}