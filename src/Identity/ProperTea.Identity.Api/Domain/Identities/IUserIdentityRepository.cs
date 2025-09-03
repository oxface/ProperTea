using ProperTea.Infrastructure.Shared.Persistence;

namespace ProperTea.Identity.Api.Domain.Identities;

public interface IUserIdentityRepository : IRepository<UserIdentity>
{
    Task<UserIdentity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserIdentity?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
}
