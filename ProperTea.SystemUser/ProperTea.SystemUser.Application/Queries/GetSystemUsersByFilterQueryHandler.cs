using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;
using ProperTea.SystemUser.Application.Models;
using ProperTea.SystemUser.Domain;

namespace ProperTea.SystemUser.Application.Queries;

public class GetSystemUsersByFilterQueryHandler(ISystemUserRepository repository)
    : IQueryHandler<GetSystemUsersByFilterQuery, PagedResult<SystemUserModel>>
{
    public async Task<PagedResult<SystemUserModel>> HandleAsync(GetSystemUsersByFilterQuery query, CancellationToken ct)
    {
        var pagedResult = await repository.GetPagedAsync(
            query.Filter,
            query.Pagination,
            query.Sort,
            ct);

        return new PagedResult<SystemUserModel>(
            pagedResult.Items.Select(c => new SystemUserModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList(),
            pagedResult.TotalCount,
            query.Pagination);
    }
}