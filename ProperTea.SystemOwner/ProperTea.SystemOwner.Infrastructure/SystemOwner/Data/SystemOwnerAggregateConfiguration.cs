using ProperTea.Shared.Infrastructure.Data.Ef;

namespace ProperTea.SystemOwner.Infrastructure.SystemOwner.Data;

public class SystemOwnerAggregateConfiguration : IAggregateConfiguration<Domain.SystemOwner.SystemOwner>
{
    public void ConfigureIncludes(IQueryable<Domain.SystemOwner.SystemOwner> query)
    {
    }
}