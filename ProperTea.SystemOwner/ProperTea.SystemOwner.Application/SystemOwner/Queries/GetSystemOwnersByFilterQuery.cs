using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;
using ProperTea.SystemOwner.Application.SystemOwner.Models;
using ProperTea.SystemOwner.Domain.SystemOwner;

namespace ProperTea.SystemOwner.Application.SystemOwner.Queries;

public class GetSystemOwnersByFilterQuery : IQuery<PagedResult<SystemOwnerModel>>
{
    public SystemOwnerFilter Filter { get; init; } = new();
    public PageRequest Pagination { get; init; } = PageRequest.Default;
    public SortRequest? Sort { get; init; }
}