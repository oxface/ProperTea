namespace ProperTea.Company.Domain.DomainEvents;

public record CompanyDeletedDomainEvent(Guid Id) : DomainEventBase
{
}