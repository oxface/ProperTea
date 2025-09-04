using Microsoft.Azure.Cosmos;
using ProperTea.Identity.Api.Domain.Identities;
using ProperTea.Shared.Infrastructure.Cosmos;

namespace ProperTea.Identity.Api.Infrastructure.Persistence;

public class CosmosUserIdentityRepository : CosmosRepository<UserIdentity>, IUserIdentityRepository
{
    public CosmosUserIdentityRepository(CosmosClient cosmosClient, string databaseName, string containerName,
        string partitionKey = "/id")
        : base(cosmosClient, databaseName, containerName, partitionKey)
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