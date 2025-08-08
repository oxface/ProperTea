using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.Shared.Domain.Exceptions;
using ProperTea.SystemOwner.Domain.SystemOwner.DomainEvents;

namespace ProperTea.SystemOwner.Domain.SystemOwner;

public class SystemOwnerDomainService(ISystemOwnerRepository repository, IDomainEventDispatcher eventDispatcher)
    : DomainServiceBase, ISystemOwnerDomainService
{
    public async Task<SystemOwner> CreateSystemOwnerAsync(string name,
        CancellationToken ct = default)
    {
        var systemOwner = SystemOwner.Create(name);
        await repository.AddAsync(systemOwner, ct);
        return systemOwner;
    }

    public async Task ChangeSystemOwnerNameAsync(Guid id, string newName, CancellationToken ct = default)
    {
        var systemOwner = await repository.GetByIdAsync(id, ct);
        if (systemOwner == null)
            throw new EntityNotFoundException(nameof(SystemOwner), id);

        systemOwner.ChangeName(newName);
    }


    public async Task DeleteSystemOwnerAsync(Guid id, CancellationToken ct = default)
    {
        var systemOwner = await repository.GetByIdAsync(id, ct);
        if (systemOwner is null)
            throw new EntityNotFoundException(nameof(SystemOwner), id);

        if (!systemOwner.AllowDelete())
            throw new InvalidOperationException("System owner cannot be deleted");
        await repository.DeleteAsync(systemOwner, ct);

        eventDispatcher.Enqueue(new SystemOwnerDeletedDomainEvent(systemOwner.Id));
    }
}