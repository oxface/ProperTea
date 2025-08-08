using Microsoft.EntityFrameworkCore;

using ProperTea.Company.Domain;
using ProperTea.Shared.Infrastructure.Data.Ef;

namespace ProperTea.Company.Infrastructure.Data;

public class CompanyRepository(CompanyDbContext context)
    : RepositoryBase<Domain.Company, CompanyFilter>(context), ICompanyRepository
{
    protected override IAggregateConfiguration<Domain.Company>? AggregateConfiguration =>
        new CompanyAggregateConfiguration();

    protected override IQueryable<Domain.Company> ApplyFilter(
        IQueryable<Domain.Company> query,
        CompanyFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(i => EF.Functions.Like(
                EF.Property<string>(i, "Name"),
                $"%{filter.Name}%"));

        return query;
    }
}