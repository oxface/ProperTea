using ProperTea.Shared.Domain;

namespace ProperTea.UserManagement.Domain.Users.ValueObjects;

public record UserIdentity : ValueObject
{
    private UserIdentity()
    {
    }

    private UserIdentity(Guid id) : this()
    {
        Id = id;
    }

    public Guid Id { get; private set; }

    public static UserIdentity Create(Guid id)
    {
        return new UserIdentity(id);
    }
}