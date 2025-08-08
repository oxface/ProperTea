namespace ProperTea.Company.Domain.DomainEvents;

public record CompanyNameChangedDomainEvent(Guid Id, string NewName) : DomainEventBase
{
}