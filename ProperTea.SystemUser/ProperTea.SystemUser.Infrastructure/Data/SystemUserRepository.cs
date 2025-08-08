using Microsoft.EntityFrameworkCore;

using ProperTea.Shared.Infrastructure.Data.Ef;
using ProperTea.SystemUser.Domain;

namespace ProperTea.SystemUser.Infrastructure.Data;

public class SystemUserRepository(SystemUserDbContext context)
    : RepositoryBase<Domain.SystemUser, SystemUserFilter>(context), ISystemUserRepository
{
    protected override IAggregateConfiguration<Domain.SystemUser>? AggregateConfiguration =>
        new SystemUserAggregateConfiguration();

    protected override IQueryable<Domain.SystemUser> ApplyFilter(
        IQueryable<Domain.SystemUser> query,
        SystemUserFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(i => EF.Functions.Like(
                EF.Property<string>(i, "Name"),
                $"%{filter.Name}%"));

        return query;
    }
}