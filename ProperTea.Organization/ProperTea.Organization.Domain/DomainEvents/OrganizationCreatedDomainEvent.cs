namespace ProperTea.Organization.Domain.DomainEvents;

public record OrganizationCreatedDomainEvent(Guid Id, string Name) : DomainEventBase
{
}