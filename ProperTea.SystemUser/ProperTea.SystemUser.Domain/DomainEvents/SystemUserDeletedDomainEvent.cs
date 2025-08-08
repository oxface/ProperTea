using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.SystemUser.Domain.DomainEvents;

public class SystemUserDeletedDomainEvent(Guid SystemUserId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}