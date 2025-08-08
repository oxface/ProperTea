using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.Company.Domain.DomainEvents;

public class CompanyNameChangedDomainEvent(Guid companyId, string newName) : IDomainEvent
{
    public Guid CompanyId { get; } = companyId;
    public string NewName { get; } = newName;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}