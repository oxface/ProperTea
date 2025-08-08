using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.Shared.Domain.ValueObjects;

public readonly record struct Organization
{
    private Organization(Guid id)
    {
        if (id == Guid.Empty)
            throw new DomainException("Organization ID cannot be empty");
        Id = id;
    }

    public Guid Id { get; }

    public static Organization Create(Guid id)
    {
        return new Organization(id);
    }
}