using ProperTea.Domain.Shared.Exceptions;

namespace ProperTea.Domain.Shared.ValueObjects;

public class PersonFullName
{
    public const int MinNameLength = 1;
    public const int MaxNameLength = 200;

    private PersonFullName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static PersonFullName Create(string value)
    {
        return value.Length switch
        {
            > MaxNameLength => throw new DomainException("PersonFullName.NameTooLong"),
            < MinNameLength => throw new DomainException("PersonFullName.NameTooShort"),
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