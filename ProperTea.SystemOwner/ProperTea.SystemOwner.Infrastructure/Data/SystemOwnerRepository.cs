using Microsoft.EntityFrameworkCore;

using ProperTea.Shared.Infrastructure.Data.Ef;
using ProperTea.SystemOwner.Domain;

namespace ProperTea.SystemOwner.Infrastructure.Data;

public class SystemOwnerRepository(SystemOwnerDbContext context)
    : RepositoryBase<Domain.SystemOwner, SystemOwnerFilter>(context), ISystemOwnerRepository
{
    protected override IAggregateConfiguration<Domain.SystemOwner>? AggregateConfiguration =>
        new SystemOwnerAggregateConfiguration();

    protected override IQueryable<Domain.SystemOwner> ApplyFilter(
        IQueryable<Domain.SystemOwner> query,
        SystemOwnerFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(i => EF.Functions.Like(
                EF.Property<string>(i, "Name"),
                $"%{filter.Name}%"));

        return query;
    }
}