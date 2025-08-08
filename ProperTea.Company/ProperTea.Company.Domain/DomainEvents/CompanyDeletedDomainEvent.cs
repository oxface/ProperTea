using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.Company.Domain.DomainEvents;

public class CompanyDeletedDomainEvent(Guid companyId) : IDomainEvent
{
    public Guid CompanyId { get; } = companyId;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}