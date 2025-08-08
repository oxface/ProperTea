namespace ProperTea.Organization.Domain.DomainEvents;

public record OrganizationNameChangedDomainEvent(Guid Id, string NewName) : DomainEventBase
{
}