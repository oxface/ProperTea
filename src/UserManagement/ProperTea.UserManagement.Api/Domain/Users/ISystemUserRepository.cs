using ProperTea.Infrastructure.Shared.Persistence;

namespace ProperTea.UserManagement.Api.Domain.Users;

public interface ISystemUserRepository : IRepository<SystemUser>
{
    Task<SystemUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
}
