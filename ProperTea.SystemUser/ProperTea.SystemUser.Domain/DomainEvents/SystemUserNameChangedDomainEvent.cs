namespace ProperTea.SystemUser.Domain.DomainEvents;

public record SystemUserNameChangedDomainEvent(Guid Id, string NewName) : DomainEventBase
{
}