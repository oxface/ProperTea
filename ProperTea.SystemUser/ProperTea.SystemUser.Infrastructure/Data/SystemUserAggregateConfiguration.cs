using ProperTea.Shared.Infrastructure.Data.Ef;

namespace ProperTea.SystemUser.Infrastructure.Data;

public class SystemUserAggregateConfiguration : IAggregateConfiguration<Domain.SystemUser>
{
    public void ConfigureIncludes(IQueryable<Domain.SystemUser> query)
    {
    }
}