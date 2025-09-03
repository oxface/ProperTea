using Microsoft.Azure.Cosmos;
using ProperTea.Infrastructure.Shared.Cosmos;
using ProperTea.Organization.Api.Domain.Organizations;

namespace ProperTea.Organization.Api.Infrastructure.Persistence;

public class CosmosOrganizationRepository : CosmosRepository<Domain.Organizations.Organization>, IOrganizationRepository
{
    public CosmosOrganizationRepository(CosmosClient cosmosClient, string databaseName, string containerName, string partitionKey = "/id") 
        : base(cosmosClient, databaseName, containerName, partitionKey)
    {
    }

    public async Task<Domain.Organizations.Organization?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT * FROM c WHERE c.Name = '{name.Replace("'", "''")}'";
        var results = await QueryAsync(query, cancellationToken);
        return results.FirstOrDefault();
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var organization = await GetByIdAsync(id, cancellationToken);
        return organization != null;
    }
}
