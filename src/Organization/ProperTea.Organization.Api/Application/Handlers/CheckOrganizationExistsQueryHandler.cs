using ProperTea.Cqrs;
using ProperTea.Organization.Api.Application.Queries;
using ProperTea.Organization.Api.Domain.Organizations;

namespace ProperTea.Organization.Api.Application.Handlers;

public class CheckOrganizationExistsQueryHandler : IQueryHandler<CheckOrganizationExistsQuery, bool>
{
    private readonly IOrganizationRepository _organizationRepository;

    public CheckOrganizationExistsQueryHandler(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<bool> HandleAsync(CheckOrganizationExistsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _organizationRepository.ExistsAsync(query.OrganizationId, cancellationToken);
    }
}