using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.SystemOwner.Domain.SystemOwner;
using ProperTea.SystemOwner.Domain.SystemOwner.DomainEvents;

namespace ProperTea.SystemOwner.Application.SystemOwner.DomainEventHandlers;

public class SystemOwnerCreatedDomainEventHandler(ISystemOwnerDomainService systemOwnerDomainService)
    : IDomainEventHandler<SystemOwnerCreatedDomainEvent>
{
    public async Task HandleAsync(SystemOwnerCreatedDomainEvent domainEvent, CancellationToken ct = default)
    {
        // TODO: integration event.
    }
}