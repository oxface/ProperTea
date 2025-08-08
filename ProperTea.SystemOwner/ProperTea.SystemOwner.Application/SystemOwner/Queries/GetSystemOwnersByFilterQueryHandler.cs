using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;
using ProperTea.SystemOwner.Application.SystemOwner.Models;
using ProperTea.SystemOwner.Domain.SystemOwner;

namespace ProperTea.SystemOwner.Application.SystemOwner.Queries;

public class GetSystemOwnersByFilterQueryHandler(ISystemOwnerRepository repository)
    : IQueryHandler<GetSystemOwnersByFilterQuery, PagedResult<SystemOwnerModel>>
{
    public async Task<PagedResult<SystemOwnerModel>> HandleAsync(GetSystemOwnersByFilterQuery query, CancellationToken ct)
    {
        var pagedResult = await repository.GetPagedAsync(
            query.Filter,
            query.Pagination,
            query.Sort,
            ct);

        return new PagedResult<SystemOwnerModel>(
            pagedResult.Items.Select(c => new SystemOwnerModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList(),
            pagedResult.TotalCount,
            query.Pagination);
    }
}