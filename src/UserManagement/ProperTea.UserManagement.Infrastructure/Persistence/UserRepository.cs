using Microsoft.EntityFrameworkCore;
using ProperTea.Shared.Infrastructure.Persistence;
using ProperTea.UserManagement.Domain.Users;

namespace ProperTea.UserManagement.Infrastructure.Persistence;

public class UserRepository(UserManagementDbContext dbContext)
    : RepositoryBase<User, UserFilter>(dbContext), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email.Value == email, ct);
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken ct = default)
    {
        return await dbContext.Users.AnyAsync(u => u.Email.Value == email, ct);
    }

    protected override IQueryable<User> ApplyFilter(IQueryable<User> query, UserFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.FullName))
            query = query.Where(e => e.FullName.Value.Contains(filter.FullName));

        return query;
    }
}

internal class UserRepositoryAggregateConfiguration : IAggregateConfiguration<User>
{
    public void ConfigureIncludes(IQueryable<User> query)
    {
        query
            .Include(q => q.UserIdentities);
    }
}