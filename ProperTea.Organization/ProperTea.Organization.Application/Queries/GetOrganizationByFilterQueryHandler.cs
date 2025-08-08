using ProperTea.Organization.Application.Models;
using ProperTea.Organization.Domain;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;

namespace ProperTea.Organization.Application.Queries;

public class GetOrganizationByFilterQueryHandler(IOrganizationRepository repository)
    : IQueryHandler<GetOrganizationByFilterQuery, PagedResult<OrganizationModel>>
{
    public async Task<PagedResult<OrganizationModel>> HandleAsync(GetOrganizationByFilterQuery query, CancellationToken ct)
    {
        var pagedResult = await repository.GetPagedAsync(
            query.Filter,
            query.Pagination,
            query.Sort,
            ct);

        return new PagedResult<OrganizationModel>(
            pagedResult.Items.Select(c => new OrganizationModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList(),
            pagedResult.TotalCount,
            query.Pagination);
    }
}