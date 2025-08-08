using ProperTea.SystemOwner.Domain.DomainEvents;
using ProperTea.SystemOwner.Domain.ValueObjects;

namespace ProperTea.SystemOwner.Domain;

public class SystemOwner : AggregateRootBase
{
    public const int MaxNameLength = 200;
    public const int MinNameLength = 1;

    private SystemOwnerName _name = null!;

    private SystemOwner()
    {
    }

    private SystemOwner(Guid id, SystemOwnerName name)
    {
        Id = id;
        _name = name;
    }

    public SystemOwnerName Name
    {
        get => _name;
        private set => _name = SystemOwnerName.Create(value);
    }

    public static SystemOwner Create(string name)
    {
        var systemOwnerName = SystemOwnerName.Create(name);
        var systemOwner = new SystemOwner(Guid.NewGuid(), systemOwnerName);
        systemOwner.AddDomainEvent(new SystemOwnerCreatedDomainEvent(systemOwner.Id, systemOwner.Name));
        return systemOwner;
    }

    public void ChangeName(string newName)
    {
        var systemOwnerName = SystemOwnerName.Create(newName);
        if (_name == systemOwnerName)
            return;

        _name = systemOwnerName;
        AddDomainEvent(new SystemOwnerNameChangedDomainEvent(Id, Name));
    }

    public bool AllowDelete()
    {
        return true;
    }
}