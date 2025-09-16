using ProperTea.Shared.Domain;
using ProperTea.Shared.Domain.Events;
using ProperTea.Shared.Domain.Exceptions;
using ProperTea.UserManagement.Domain.UserGroups.ValueObjects;

namespace ProperTea.UserManagement.Domain.UserGroups;

public class UserGroup : AggregateRoot
{
    private readonly IList<Guid> usersIds = [];

    private UserGroup() : base(Guid.Empty)
    {
    } // For deserialization

    public UserGroup(Guid id, string name, string description)
        : base(id)
    {
        Id = id;
        Name = UserGroupName.Create(name);
        Description = UserGroupDescription.Create(description);

        RaiseDomainEvent(new UserGroupCreatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            Name,
            Description));
    }

    public UserGroupName Name { get; private set; } = null!;
    public UserGroupDescription Description { get; private set; } = null!;

    public IReadOnlyCollection<Guid> UserIds => usersIds.AsReadOnly();

    public static UserGroup Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("UserGroup.NameRequired");

        return new UserGroup(Guid.NewGuid(), name, description);
    }

    public void ChangeDetails(string name, string description)
    {
        Name = UserGroupName.Create(name);
        Description = UserGroupDescription.Create(description);

        RaiseDomainEvent(new UserGroupChangedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            name,
            description));
    }

    public void AddUser(Guid userId)
    {
        usersIds.Add(userId);

        RaiseDomainEvent(new UserAddedToUserGroupDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            userId));
    }

    public void RemoveUser(Guid userId)
    {
        Guid? user = usersIds.FirstOrDefault(m => m == userId);
        if (user == null)
            return;

        usersIds.Remove(user.Value);

        RaiseDomainEvent(new UserRemovedUserGroupDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            userId));
    }
}

public record UserGroupCreatedDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserGroupId,
    string Name,
    string Description
) : DomainEvent(Id, OccurredAt);

public record UserGroupChangedDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserGroupId,
    string Name,
    string Description
) : DomainEvent(Id, OccurredAt);

public record UserAddedToUserGroupDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserGroupId,
    Guid UserId
) : DomainEvent(Id, OccurredAt);

public record UserRemovedUserGroupDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserGroupId,
    Guid UserId
) : DomainEvent(Id, OccurredAt);