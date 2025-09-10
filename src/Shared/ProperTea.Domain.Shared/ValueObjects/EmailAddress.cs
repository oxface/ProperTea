using ProperTea.Domain.Shared.Exceptions;

namespace ProperTea.Domain.Shared.ValueObjects;

public record EmailAddress : ValueObject
{
    public const int MinNameLength = 1;
    public const int MaxNameLength = 200;

    private EmailAddress(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("EmailAddress.Required");

        return value.Length switch
        {
            > MaxNameLength => throw new DomainException("EmailAddress.NameTooLong"),
            < MinNameLength => throw new DomainException("EmailAddress.NameTooShort"),
            _ => new EmailAddress(value)
        };
    }

    public static implicit operator string(EmailAddress name)
    {
        return name.Value;
    }

    public static explicit operator EmailAddress(string value)
    {
        return Create(value);
    }
}