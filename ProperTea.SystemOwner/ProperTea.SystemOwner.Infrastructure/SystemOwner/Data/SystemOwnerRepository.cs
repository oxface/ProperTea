using ProperTea.Shared.Infrastructure.Data.Ef;
using ProperTea.SystemOwner.Domain.SystemOwner;

namespace ProperTea.SystemOwner.Infrastructure.SystemOwner.Data;

public class SystemOwnerRepository(SystemOwnerDbContext context)
    : RepositoryBase<Domain.SystemOwner.SystemOwner, SystemOwnerFilter>(context), ISystemOwnerRepository
{
    protected override IAggregateConfiguration<Domain.SystemOwner.SystemOwner>? AggregateConfiguration =>
        new SystemOwnerAggregateConfiguration();

    protected override IQueryable<Domain.SystemOwner.SystemOwner> ApplyFilter(
        IQueryable<Domain.SystemOwner.SystemOwner> query,
        SystemOwnerFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(i => i.Name.Value.Contains(filter.Name));

        return query;
    }
}