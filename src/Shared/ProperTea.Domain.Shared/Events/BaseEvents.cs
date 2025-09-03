namespace ProperTea.Contracts.Events;

public abstract record DomainEvent(Guid Id, DateTime OccurredAt);

public abstract record IntegrationEvent(Guid Id, DateTime OccurredAt);
