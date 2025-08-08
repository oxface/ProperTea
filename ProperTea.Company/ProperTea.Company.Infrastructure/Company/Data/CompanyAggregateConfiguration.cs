using ProperTea.Shared.Infrastructure.Data.Ef;

namespace ProperTea.Company.Infrastructure.Company.Data;

public class CompanyAggregateConfiguration : IAggregateConfiguration<Domain.Company.Company>
{
    public void ConfigureIncludes(IQueryable<Domain.Company.Company> query)
    {
    }
}