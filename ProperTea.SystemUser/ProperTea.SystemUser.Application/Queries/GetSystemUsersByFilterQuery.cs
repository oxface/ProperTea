using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;
using ProperTea.SystemUser.Application.Models;
using ProperTea.SystemUser.Domain;

namespace ProperTea.SystemUser.Application.Queries;

public class GetSystemUsersByFilterQuery : IQuery<PagedResult<SystemUserModel>>
{
    public SystemUserFilter Filter { get; init; } = new();
    public PageRequest Pagination { get; init; } = PageRequest.Default;
    public SortRequest? Sort { get; init; }
}