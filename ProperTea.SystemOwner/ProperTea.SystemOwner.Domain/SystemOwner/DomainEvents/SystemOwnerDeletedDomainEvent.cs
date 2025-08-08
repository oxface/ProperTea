using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.SystemOwner.Domain.SystemOwner.DomainEvents;

public class SystemOwnerDeletedDomainEvent(Guid systemOwnerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}