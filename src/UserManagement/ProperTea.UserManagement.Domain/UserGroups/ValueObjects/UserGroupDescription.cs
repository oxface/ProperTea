using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.UserManagement.Domain.UserGroups.ValueObjects;

public class UserGroupDescription
{
    public const int MaxLength = 200;

    private UserGroupDescription(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static UserGroupDescription Create(string value)
    {
        return value.Length switch
        {
            > MaxLength => throw new DomainException("UserGroupDescription.NameTooLong"),
            _ => new UserGroupDescription(value)
        };
    }

    public static implicit operator string(UserGroupDescription name)
    {
        return name.Value;
    }

    public static explicit operator UserGroupDescription(string value)
    {
        return Create(value);
    }
}