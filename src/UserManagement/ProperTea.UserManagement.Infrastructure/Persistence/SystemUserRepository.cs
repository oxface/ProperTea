using Microsoft.EntityFrameworkCore;
using ProperTea.Infrastructure.Shared.Persistence;
using ProperTea.UserManagement.Domain.SystemUsers;

namespace ProperTea.UserManagement.Infrastructure.Persistence;

public class SystemUserRepository(UserManagementDbContext dbContext)
    : RepositoryBase<SystemUser, object>(dbContext), ISystemUserRepository
{
    public async Task<SystemUser?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await dbContext.SystemUsers.FirstOrDefaultAsync(u => u.Email.Value == email, ct);
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken ct = default)
    {
        var user = await GetByEmailAsync(email, ct);
        return user != null;
    }

    protected override IQueryable<SystemUser> ApplyFilter(IQueryable<SystemUser> query, object filter)
    {
        return query;
    }
}

internal class SystemUserRepositoryAggregateConfiguration : IAggregateConfiguration<SystemUser>
{
    public void ConfigureIncludes(IQueryable<SystemUser> query)
    {
        query
            .Include(q => q.OrganizationMemberships)
            .Include(q => q.UserIdentities);
    }
}