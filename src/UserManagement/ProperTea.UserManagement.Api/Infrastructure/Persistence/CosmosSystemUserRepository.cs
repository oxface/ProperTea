using Microsoft.Azure.Cosmos;
using ProperTea.Shared.Infrastructure.Cosmos;
using ProperTea.UserManagement.Api.Domain.Users;

namespace ProperTea.UserManagement.Api.Infrastructure.Persistence;

public class CosmosSystemUserRepository : CosmosRepository<SystemUser>, ISystemUserRepository
{
    public CosmosSystemUserRepository(CosmosClient cosmosClient, string databaseName, string containerName,
        string partitionKey = "/id")
        : base(cosmosClient, databaseName, containerName, partitionKey)
    {
    }

    public async Task<SystemUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT * FROM c WHERE c.Email = '{email.ToLowerInvariant().Replace("'", "''")}'";
        var results = await QueryAsync(query, cancellationToken);
        return results.FirstOrDefault();
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await GetByEmailAsync(email, cancellationToken);
        return user != null;
    }
}