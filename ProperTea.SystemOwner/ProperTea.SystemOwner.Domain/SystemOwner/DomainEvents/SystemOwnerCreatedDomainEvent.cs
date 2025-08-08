using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.SystemOwner.Domain.SystemOwner.DomainEvents;

public class SystemOwnerCreatedDomainEvent(Guid systemOwnerId, string name) : IDomainEvent
{
    public Guid SystemOwnerId { get; } = systemOwnerId;
    public string Name { get; } = name;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}