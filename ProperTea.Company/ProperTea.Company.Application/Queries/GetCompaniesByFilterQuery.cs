using ProperTea.Company.Application.Models;
using ProperTea.Company.Domain;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;

namespace ProperTea.Company.Application.Queries;

public class GetCompaniesByFilterQuery : IQuery<PagedResult<CompanyModel>>
{
    public CompanyFilter Filter { get; init; } = new();
    public PageRequest Pagination { get; init; } = PageRequest.Default;
    public SortRequest? Sort { get; init; }
}