using ProperTea.Shared.Infrastructure.Data.Ef;

namespace ProperTea.Company.Infrastructure.Data;

public class CompanyAggregateConfiguration : IAggregateConfiguration<Domain.Company>
{
    public void ConfigureIncludes(IQueryable<Domain.Company> query)
    {
    }
}