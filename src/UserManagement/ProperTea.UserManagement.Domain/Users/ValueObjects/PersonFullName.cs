using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.UserManagement.Domain.Users.ValueObjects;

public class PersonFullName
{
    public const int MinLength = 1;
    public const int MaxLength = 200;

    private PersonFullName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static PersonFullName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("PersonFullName.Required");

        return value.Length switch
        {
            > MaxLength => throw new DomainException("PersonFullName.NameTooLong"),
            < MinLength => throw new DomainException("PersonFullName.NameTooShort"),
            _ => new PersonFullName(value)
        };
    }

    public static implicit operator string(PersonFullName name)
    {
        return name.Value;
    }

    public static explicit operator PersonFullName(string value)
    {
        return Create(value);
    }
}