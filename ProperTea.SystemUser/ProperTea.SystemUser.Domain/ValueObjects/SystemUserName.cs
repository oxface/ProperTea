using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.SystemUser.Domain.ValueObjects;

public record SystemUserName
{
    private SystemUserName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static SystemUserName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("SystemUser.NameRequired");

        return value.Length switch
        {
            > SystemUser.MaxNameLength => throw new DomainException("SystemUser.NameTooLong"),
            < SystemUser.MinNameLength => throw new DomainException("SystemUser.NameTooShort"),
            _ => new SystemUserName(value)
        };
    }

    public static implicit operator string(SystemUserName name)
    {
        return name.Value;
    }

    public static explicit operator SystemUserName(string value)
    {
        return Create(value);
    }
}