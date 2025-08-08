using ProperTea.Organization.Application.Models;
using ProperTea.Organization.Domain;
using ProperTea.Shared.Application.Queries;

namespace ProperTea.Organization.Application.Queries;

public class GetOrganizationByIdQueryHandler(IOrganizationRepository repository)
    : IQueryHandler<GetOrganizationByIdQuery, OrganizationModel>
{
    public async Task<OrganizationModel> HandleAsync(GetOrganizationByIdQuery query, CancellationToken ct)
    {
        var organization = await repository.GetByIdAsync(query.Id, ct);
        if (organization == null)
            throw new Exception("Organization not found");
        return new OrganizationModel
        {
            Id = organization.Id,
            Name = organization.Name
        };
    }
}