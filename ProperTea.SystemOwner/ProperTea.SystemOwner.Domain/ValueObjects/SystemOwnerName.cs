using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.SystemOwner.Domain.ValueObjects;

public record SystemOwnerName
{
    private SystemOwnerName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static SystemOwnerName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("SystemOwner.NameRequired");

        return value.Length switch
        {
            > SystemOwner.MaxNameLength => throw new DomainException("SystemOwner.NameTooLong"),
            < SystemOwner.MinNameLength => throw new DomainException("SystemOwner.NameTooShort"),
            _ => new SystemOwnerName(value)
        };
    }

    public static implicit operator string(SystemOwnerName name)
    {
        return name.Value;
    }

    public static explicit operator SystemOwnerName(string value)
    {
        return Create(value);
    }
}