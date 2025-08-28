using ProperTea.Infrastructure.Persistence;

namespace ProperTea.UserManagement.Api.Domain.Users;

public interface ISystemUserRepository : IRepository<SystemUser>
{
    Task<SystemUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<SystemUser>> GetByOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
