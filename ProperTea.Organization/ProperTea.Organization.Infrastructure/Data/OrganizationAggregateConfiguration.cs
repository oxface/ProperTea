using ProperTea.Shared.Infrastructure.Data.Ef;

namespace ProperTea.Organization.Infrastructure.Data;

public class OrganizationAggregateConfiguration : IAggregateConfiguration<Domain.Organization>
{
    public void ConfigureIncludes(IQueryable<Domain.Organization> query)
    {
    }
}