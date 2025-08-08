using ProperTea.Organization.Domain;
using ProperTea.Organization.Domain.DomainEvents;
using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.Organization.Application.DomainEventHandlers;

public class OrganizationCreatedDomainEventHandler(IOrganizationDomainService organizationDomainService)
    : IDomainEventHandler<OrganizationCreatedDomainEvent>
{
    public async Task HandleAsync(OrganizationCreatedDomainEvent domainEvent, CancellationToken ct = default)
    {
        // TODO: integration event.
    }
}