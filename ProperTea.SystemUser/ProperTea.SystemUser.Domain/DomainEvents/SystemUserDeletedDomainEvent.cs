namespace ProperTea.SystemUser.Domain.DomainEvents;

public record SystemUserDeletedDomainEvent(Guid Id) : DomainEventBase
{
}