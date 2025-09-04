using System.Net;
using Microsoft.Azure.Cosmos;
using ProperTea.Shared.Infrastructure.Domain;
using ProperTea.Shared.Infrastructure.Persistence;

namespace ProperTea.Shared.Infrastructure.Cosmos;

public class CosmosRepository<T> : ICosmosRepository<T> where T : AggregateRoot
{
    private readonly Container _container;
    private readonly string _partitionKey;

    public CosmosRepository(CosmosClient cosmosClient, string databaseName, string containerName,
        string partitionKey = "/id")
    {
        _container = cosmosClient.GetContainer(databaseName, containerName);
        _partitionKey = partitionKey;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _container.ReadItemAsync<T>(
                id.ToString(),
                new PartitionKey(id.ToString()),
                cancellationToken: cancellationToken);

            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task SaveAsync(T aggregate, CancellationToken cancellationToken = default)
    {
        await _container.UpsertItemAsync(
            aggregate,
            new PartitionKey(aggregate.Id.ToString()),
            cancellationToken: cancellationToken);

        aggregate.ClearDomainEvents();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _container.DeleteItemAsync<T>(
            id.ToString(),
            new PartitionKey(id.ToString()),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<T>> QueryAsync(string query, CancellationToken cancellationToken = default)
    {
        var queryDefinition = new QueryDefinition(query);
        var queryIterator = _container.GetItemQueryIterator<T>(queryDefinition);

        var results = new List<T>();
        while (queryIterator.HasMoreResults)
        {
            var response = await queryIterator.ReadNextAsync(cancellationToken);
            results.AddRange(response);
        }

        return results;
    }
}