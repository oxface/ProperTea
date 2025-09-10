namespace ProperTea.Domain.Shared.Events;

public abstract record IntegrationEvent(Guid Id, DateTime OccurredAt);