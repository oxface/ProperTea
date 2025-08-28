using Microsoft.Azure.Cosmos;
using ProperTea.Infrastructure.Cosmos;
using ProperTea.Identity.Api.Domain.Identities;

namespace ProperTea.Identity.Api.Infrastructure.Persistence;

public class CosmosUserIdentityRepository : CosmosRepository<UserIdentity>, IUserIdentityRepository
{
    public CosmosUserIdentityRepository(Container container) : base(container)
    {
    }

    public async Task<UserIdentity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT * FROM c WHERE c.Email = '{email.ToLowerInvariant().Replace("'", "''")}'";
        var results = await QueryAsync(query, cancellationToken);
        return results.FirstOrDefault();
    }

    public async Task<UserIdentity?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT * FROM c WHERE c.UserId = '{userId}'";
        var results = await QueryAsync(query, cancellationToken);
        return results.FirstOrDefault();
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        var identity = await GetByEmailAsync(email, cancellationToken);
        return identity != null;
    }
}
