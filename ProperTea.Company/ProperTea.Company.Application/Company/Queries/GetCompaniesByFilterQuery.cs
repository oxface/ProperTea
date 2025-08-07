using ProperTea.Company.Application.Company.Models;
using ProperTea.Company.Domain.Company;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;

namespace ProperTea.Company.Application.Company.Queries;

public class GetCompaniesByFilterQuery : IQuery<PagedResult<CompanyModel>>
{
    public CompanyFilter Filter { get; init; } = new();
    public PageRequest Pagination { get; init; } = PageRequest.Default;
    public SortRequest? Sort { get; init; }
}