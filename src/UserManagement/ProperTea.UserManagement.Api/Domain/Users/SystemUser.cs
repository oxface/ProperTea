using ProperTea.Infrastructure.Shared.Domain;
using ProperTea.Contracts.Events;

namespace ProperTea.UserManagement.Api.Domain.Users;

public class SystemUser : AggregateRoot
{
    public string Email { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    
    private readonly List<OrganizationMembership> _organizationMemberships = [];
    public IReadOnlyCollection<OrganizationMembership> OrganizationMemberships => _organizationMemberships.AsReadOnly();

    private SystemUser() { } // For deserialization

    private SystemUser(Guid id, string email, string fullName)
    {
        Id = id;
        Email = email;
        FullName = fullName;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;

        RaiseDomainEvent(new UserCreatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            Email,
            FullName));
    }

    public static SystemUser Create(string email, string fullName)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
        
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty", nameof(fullName));

        return new SystemUser(Guid.NewGuid(), email.ToLowerInvariant(), fullName);
    }

    public void AddOrganizationMembership(Guid organizationId, UserRole role)
    {
        if (_organizationMemberships.Any(m => m.OrganizationId == organizationId))
            return;

        var membership = new OrganizationMembership(Guid.NewGuid(), organizationId, role, DateTime.UtcNow);
        _organizationMemberships.Add(membership);

        RaiseDomainEvent(new UserAddedToOrganizationDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            organizationId,
            role));
    }

    public void UpdateProfile(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty", nameof(fullName));

        FullName = fullName;
    }
}

public class OrganizationMembership
{
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }

    private OrganizationMembership() { } // For deserialization

    public OrganizationMembership(Guid id, Guid organizationId, UserRole role, DateTime joinedAt)
    {
        Id = id;
        OrganizationId = organizationId;
        Role = role;
        JoinedAt = joinedAt;
    }
}

public enum UserRole
{
    Admin,
    User,
    Viewer
}

public record UserCreatedDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserId,
    string Email,
    string FullName
) : DomainEvent(Id, OccurredAt);

public record UserAddedToOrganizationDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserId,
    Guid OrganizationId,
    UserRole Role
) : DomainEvent(Id, OccurredAt);
