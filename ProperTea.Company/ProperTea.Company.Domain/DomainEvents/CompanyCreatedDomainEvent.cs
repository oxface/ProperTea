using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.Company.Domain.DomainEvents;

public class CompanyCreatedDomainEvent(Guid companyId, string name) : IDomainEvent
{
    public Guid CompanyId { get; } = companyId;
    public string Name { get; } = name;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}