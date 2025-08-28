using ProperTea.Infrastructure.Domain;
using ProperTea.Contracts.Events;

namespace ProperTea.Organization.Api.Domain.Organizations;

public class Organization : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CreatedByUserId { get; private set; }

    private Organization() { } // For deserialization

    private Organization(Guid id, string name, string description, Guid createdByUserId)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedByUserId = createdByUserId;
        CreatedAt = DateTime.UtcNow;
        IsActive = false; // Start as pending

        RaiseDomainEvent(new OrganizationCreatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            Name,
            CreatedByUserId));
    }

    public static Organization Create(string name, string description, Guid createdByUserId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Organization name cannot be empty", nameof(name));

        return new Organization(Guid.NewGuid(), name, description, createdByUserId);
    }

    public void Activate()
    {
        if (IsActive) return;

        IsActive = true;
        RaiseDomainEvent(new OrganizationActivatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id));
    }

    public void UpdateDetails(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Organization name cannot be empty", nameof(name));

        Name = name;
        Description = description;
    }
}

public record OrganizationCreatedDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid OrganizationId,
    string Name,
    Guid CreatedByUserId
) : DomainEvent(Id, OccurredAt);

public record OrganizationActivatedDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid OrganizationId
) : DomainEvent(Id, OccurredAt);
