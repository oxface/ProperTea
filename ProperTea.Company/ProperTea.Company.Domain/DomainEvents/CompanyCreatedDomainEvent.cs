namespace ProperTea.Company.Domain.DomainEvents;

public record CompanyCreatedDomainEvent(Guid Id, string Name) : DomainEventBase
{
}