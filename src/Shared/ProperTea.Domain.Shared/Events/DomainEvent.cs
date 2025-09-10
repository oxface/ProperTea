namespace ProperTea.Domain.Shared.Events;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredAt { get; }
}

public interface IPrioritizedDomainEvent : IDomainEvent
{
    int Priority { get; }
}

public abstract record DomainEvent(Guid Id, DateTime OccurredAt) : IDomainEvent;