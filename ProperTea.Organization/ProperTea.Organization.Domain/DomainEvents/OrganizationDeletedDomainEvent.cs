namespace ProperTea.Organization.Domain.DomainEvents;

public record OrganizationDeletedDomainEvent(Guid Id) : DomainEventBase
{
}