using ProperTea.SystemUser.Domain.DomainEvents;
using ProperTea.SystemUser.Domain.ValueObjects;

namespace ProperTea.SystemUser.Domain;

public class SystemUser : AggregateRootBase
{
    public const int MaxNameLength = 200;
    public const int MinNameLength = 1;

    private SystemUserName _name = null!;

    private SystemUser()
    {
    }

    private SystemUser(Guid id, SystemUserName name)
    {
        Id = id;
        _name = name;
    }

    public SystemUserName Name
    {
        get => _name;
        private set => _name = SystemUserName.Create(value);
    }

    public static SystemUser Create(string name)
    {
        var systemUserName = SystemUserName.Create(name);
        var systemUser = new SystemUser(Guid.NewGuid(), systemUserName);
        systemUser.AddDomainEvent(new SystemUserCreatedDomainEvent(systemUser.Id, systemUser.Name));
        return systemUser;
    }

    public void ChangeName(string newName)
    {
        var systemUserName = SystemUserName.Create(newName);
        if (_name == systemUserName)
            return;

        _name = systemUserName;
        AddDomainEvent(new SystemUserNameChangedDomainEvent(Id, Name));
    }

    public bool AllowDelete()
    {
        return true;
    }
}