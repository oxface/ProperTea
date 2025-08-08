using ProperTea.Company.Domain.Company;
using ProperTea.Shared.Infrastructure.Data.Ef;

namespace ProperTea.Company.Infrastructure.Company.Data;

public class CompanyRepository(CompanyDbContext context)
    : RepositoryBase<Domain.Company.Company, CompanyFilter>(context), ICompanyRepository
{
    protected override IAggregateConfiguration<Domain.Company.Company>? AggregateConfiguration =>
        new CompanyAggregateConfiguration();

    protected override IQueryable<Domain.Company.Company> ApplyFilter(
        IQueryable<Domain.Company.Company> query,
        CompanyFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(i => i.Name.Value.Contains(filter.Name));

        return query;
    }
}