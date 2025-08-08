namespace ProperTea.SystemUser.Domain.DomainEvents;

public record SystemUserCreatedDomainEvent(Guid Id, string Name) : DomainEventBase
{
}