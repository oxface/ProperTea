using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.SystemUser.Domain.DomainEvents;

public class SystemUserCreatedDomainEvent(Guid systemUserId, string name) : IDomainEvent
{
    public Guid SystemUserId { get; } = systemUserId;
    public string Name { get; } = name;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}