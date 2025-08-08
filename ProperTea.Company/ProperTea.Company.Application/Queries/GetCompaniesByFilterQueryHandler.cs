using ProperTea.Company.Application.Models;
using ProperTea.Company.Domain;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;

namespace ProperTea.Company.Application.Queries;

public class GetCompaniesByFilterQueryHandler(ICompanyRepository repository)
    : IQueryHandler<GetCompaniesByFilterQuery, PagedResult<CompanyModel>>
{
    public async Task<PagedResult<CompanyModel>> HandleAsync(GetCompaniesByFilterQuery query, CancellationToken ct)
    {
        var pagedResult = await repository.GetPagedAsync(
            query.Filter,
            query.Pagination,
            query.Sort,
            ct);

        return new PagedResult<CompanyModel>(
            pagedResult.Items.Select(c => new CompanyModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList(),
            pagedResult.TotalCount,
            query.Pagination);
    }
}