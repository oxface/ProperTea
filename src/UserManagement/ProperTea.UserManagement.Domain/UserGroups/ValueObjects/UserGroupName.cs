using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.UserManagement.Domain.UserGroups.ValueObjects;

public class UserGroupName
{
    public const int MinLength = 1;
    public const int MaxLength = 40;

    private UserGroupName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static UserGroupName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("UserGroupName.Required");

        return value.Length switch
        {
            > MaxLength => throw new DomainException("UserGroupName.NameTooLong"),
            < MinLength => throw new DomainException("UserGroupName.NameTooShort"),
            _ => new UserGroupName(value)
        };
    }

    public static implicit operator string(UserGroupName name)
    {
        return name.Value;
    }

    public static explicit operator UserGroupName(string value)
    {
        return Create(value);
    }
}