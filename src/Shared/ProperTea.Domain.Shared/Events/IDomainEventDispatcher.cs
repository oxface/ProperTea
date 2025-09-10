namespace ProperTea.Domain.Shared.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAllAsync(CancellationToken cancellationToken = default);
    void Enqueue(IDomainEvent domainEvent);
}