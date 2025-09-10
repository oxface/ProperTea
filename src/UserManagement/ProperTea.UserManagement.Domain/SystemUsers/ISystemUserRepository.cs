using ProperTea.Domain.Shared;

namespace ProperTea.UserManagement.Domain.SystemUsers;

public interface ISystemUserRepository : IRepository<SystemUser, object>
{
    Task<SystemUser?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsAsync(string email, CancellationToken ct = default);
}