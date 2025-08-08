using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.Organization.Domain.ValueObjects;

public record OrganizationName
{
    private OrganizationName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static OrganizationName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Organization.NameRequired");

        return value.Length switch
        {
            > Organization.MaxNameLength => throw new DomainException("Organization.NameTooLong"),
            < Organization.MinNameLength => throw new DomainException("Organization.NameTooShort"),
            _ => new OrganizationName(value)
        };
    }

    public static implicit operator string(OrganizationName name)
    {
        return name.Value;
    }

    public static explicit operator OrganizationName(string value)
    {
        return Create(value);
    }
}