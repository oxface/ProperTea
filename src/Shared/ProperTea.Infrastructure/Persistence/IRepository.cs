using ProperTea.Infrastructure.Domain;

namespace ProperTea.Infrastructure.Persistence;

public interface IRepository<T> where T : AggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(T aggregate, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface ICosmosRepository<T> : IRepository<T> where T : AggregateRoot
{
    Task<IEnumerable<T>> QueryAsync(string query, CancellationToken cancellationToken = default);
}
