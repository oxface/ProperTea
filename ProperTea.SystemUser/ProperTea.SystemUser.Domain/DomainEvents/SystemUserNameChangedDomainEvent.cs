using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.SystemUser.Domain.DomainEvents;

public class SystemUserNameChangedDomainEvent(Guid systemUserId, string newName) : IDomainEvent
{
    public Guid SystemUserId { get; } = systemUserId;
    public string NewName { get; } = newName;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}