using ProperTea.Shared.Infrastructure.Data.Ef;

namespace ProperTea.SystemOwner.Infrastructure.Data;

public class SystemOwnerAggregateConfiguration : IAggregateConfiguration<Domain.SystemOwner>
{
    public void ConfigureIncludes(IQueryable<Domain.SystemOwner> query)
    {
    }
}