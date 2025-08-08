using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.SystemOwner.Domain;
using ProperTea.SystemOwner.Domain.DomainEvents;

namespace ProperTea.SystemOwner.Application.DomainEventHandlers;

public class SystemOwnerCreatedDomainEventHandler(ISystemOwnerDomainService systemOwnerDomainService)
    : IDomainEventHandler<SystemOwnerCreatedDomainEvent>
{
    public async Task HandleAsync(SystemOwnerCreatedDomainEvent domainEvent, CancellationToken ct = default)
    {
        // TODO: integration event.
    }
}