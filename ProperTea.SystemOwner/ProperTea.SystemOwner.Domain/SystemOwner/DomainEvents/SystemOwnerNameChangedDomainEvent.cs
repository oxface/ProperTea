using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.SystemOwner.Domain.SystemOwner.DomainEvents;

public class SystemOwnerNameChangedDomainEvent(Guid systemOwnerId, string newName) : IDomainEvent
{
    public Guid SystemOwnerId { get; } = systemOwnerId;
    public string NewName { get; } = newName;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}