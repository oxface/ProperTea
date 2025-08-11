using ProperTea.Organization.Domain.DomainEvents;
using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.Organization.Domain;

public class OrganizationDomainService(IOrganizationRepository repository, IDomainEventDispatcher eventDispatcher)
    : DomainServiceBase, IOrganizationDomainService
{
    public async Task<Organization> CreateOrganizationAsync(
        string name,
        CancellationToken ct = default)
    {
        var organization = Organization.Create(name);
        await repository.AddAsync(organization, ct);
        return organization;
    }

    public async Task ChangeOrganizationNameAsync(Guid id, string newName, CancellationToken ct = default)
    {
        var organization = await repository.GetByIdAsync(id, ct);
        if (organization == null)
            throw new EntityNotFoundException(nameof(Organization), id);

        organization.ChangeName(newName);
    }


    public async Task DeleteOrganizationAsync(Guid id, CancellationToken ct = default)
    {
        var organization = await repository.GetByIdAsync(id, ct);
        if (organization is null)
            throw new EntityNotFoundException(nameof(Organization), id);

        if (!organization.AllowDelete())
            throw new InvalidOperationException("System owner cannot be deleted");
        
        await repository.DeleteAsync(organization, ct);

        eventDispatcher.Enqueue(new OrganizationDeletedDomainEvent(organization.Id));
    }
}