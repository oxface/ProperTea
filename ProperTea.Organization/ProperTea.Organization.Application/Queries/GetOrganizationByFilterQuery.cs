using ProperTea.Organization.Application.Models;
using ProperTea.Organization.Domain;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;

namespace ProperTea.Organization.Application.Queries;

public class GetOrganizationByFilterQuery : IQuery<PagedResult<OrganizationModel>>
{
    public OrganizationFilter Filter { get; init; } = new();
    public PageRequest Pagination { get; init; } = PageRequest.Default;
    public SortRequest? Sort { get; init; }
}