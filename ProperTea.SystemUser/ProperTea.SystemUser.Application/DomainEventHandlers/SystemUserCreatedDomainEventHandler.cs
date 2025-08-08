using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.SystemUser.Domain;
using ProperTea.SystemUser.Domain.DomainEvents;

namespace ProperTea.SystemUser.Application.DomainEventHandlers;

public class SystemUserCreatedDomainEventHandler(ISystemUserDomainService systemUserDomainService)
    : IDomainEventHandler<SystemUserCreatedDomainEvent>
{
    public async Task HandleAsync(SystemUserCreatedDomainEvent domainEvent, CancellationToken ct = default)
    {
        // TODO: integration event.
    }
}