using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.Shared.Domain;

public record DomainEventBase : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}