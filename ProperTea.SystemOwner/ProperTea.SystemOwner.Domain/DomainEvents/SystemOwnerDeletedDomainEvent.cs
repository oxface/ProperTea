using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.SystemOwner.Domain.DomainEvents;

public class SystemOwnerDeletedDomainEvent(Guid SystemOwnerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}