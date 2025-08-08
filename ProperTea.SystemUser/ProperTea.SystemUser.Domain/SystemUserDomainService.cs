using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.Shared.Domain.Exceptions;
using ProperTea.SystemUser.Domain.DomainEvents;

namespace ProperTea.SystemUser.Domain;

public class SystemUserDomainService(ISystemUserRepository repository, IDomainEventDispatcher eventDispatcher)
    : DomainServiceBase, ISystemUserDomainService
{
    public async Task<SystemUser> CreateSystemUserAsync(
        string name,
        CancellationToken ct = default)
    {
        var systemUser = SystemUser.Create(name);
        await repository.AddAsync(systemUser, ct);
        return systemUser;
    }

    public async Task ChangeSystemUserNameAsync(Guid id, string newName, CancellationToken ct = default)
    {
        var systemUser = await repository.GetByIdAsync(id, ct);
        if (systemUser == null)
            throw new EntityNotFoundException(nameof(SystemUser), id);

        systemUser.ChangeName(newName);
    }


    public async Task DeleteSystemUserAsync(Guid id, CancellationToken ct = default)
    {
        var systemUser = await repository.GetByIdAsync(id, ct);
        if (systemUser is null)
            throw new EntityNotFoundException(nameof(SystemUser), id);

        if (!systemUser.AllowDelete())
            throw new InvalidOperationException("System owner cannot be deleted");
        await repository.DeleteAsync(systemUser, ct);

        eventDispatcher.Enqueue(new SystemUserDeletedDomainEvent(systemUser.Id));
    }
}