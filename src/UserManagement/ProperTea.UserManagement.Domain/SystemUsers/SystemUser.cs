using ProperTea.Domain.Shared;
using ProperTea.Domain.Shared.Events;
using ProperTea.Domain.Shared.ValueObjects;

namespace ProperTea.UserManagement.Domain.SystemUsers;

public class SystemUser : AggregateRoot
{
    private readonly List<OrganizationMembership> organizationMemberships = [];
    private readonly List<UserIdentity> userIdentities = [];

    private SystemUser() : base(Guid.Empty)
    {
    } // For deserialization

    private SystemUser(Guid id, string email, string fullName) : base(id)
    {
        Id = id;
        Email = EmailAddress.Create(email);
        FullName = PersonFullName.Create(fullName);
        CreatedAt = DateTime.UtcNow;
        IsActive = true;

        RaiseDomainEvent(new SystemUserCreatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            Email,
            FullName));
    }

    public EmailAddress Email { get; } = null!;
    public PersonFullName FullName { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<OrganizationMembership> OrganizationMemberships => organizationMemberships.AsReadOnly();

    public IReadOnlyCollection<UserIdentity> UserIdentities => userIdentities.AsReadOnly();

    public static SystemUser Create(string email, string fullName)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty", nameof(fullName));

        return new SystemUser(Guid.NewGuid(), email.ToLowerInvariant(), fullName);
    }

    public void AddOrganizationMembership(Guid organizationId, SystemUserOrganizationRole organizationRole)
    {
        if (organizationMemberships.Any(m => m.OrganizationId == organizationId))
            return;

        var membership = new OrganizationMembership(Guid.NewGuid(), organizationId, organizationRole, DateTime.UtcNow);
        organizationMemberships.Add(membership);

        RaiseDomainEvent(new SystemUserAddedToOrganizationDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            organizationId,
            organizationRole));
    }

    public void AddUserIdentity(Guid userIdentityId)
    {
        if (userIdentities.Any(m => m.Id == userIdentityId))
            return;

        var userIdentity = new UserIdentity(userIdentityId);
        userIdentities.Add(userIdentity);

        RaiseDomainEvent(new UserIdentityAddedToSystemUserDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            userIdentity.Id));
    }

    public void UpdateProfile(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty", nameof(fullName));

        FullName = PersonFullName.Create(fullName);
    }
}

public class OrganizationMembership
{
    private OrganizationMembership()
    {
    } // For deserialization

    public OrganizationMembership(Guid id, Guid organizationId, SystemUserOrganizationRole organizationRole,
        DateTime joinedAt)
    {
        Id = id;
        OrganizationId = organizationId;
        OrganizationRole = organizationRole;
        JoinedAt = joinedAt;
    }

    public Guid Id { get; private set; }
    public Guid OrganizationId { get; }
    public SystemUserOrganizationRole OrganizationRole { get; private set; }
    public DateTime JoinedAt { get; private set; }
}

public enum SystemUserOrganizationRole
{
    Admin,
    User,
    Viewer
}

public record SystemUserCreatedDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserId,
    string Email,
    string FullName
) : DomainEvent(Id, OccurredAt);

public record SystemUserAddedToOrganizationDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserId,
    Guid OrganizationId,
    SystemUserOrganizationRole OrganizationRole
) : DomainEvent(Id, OccurredAt);

public record UserIdentityAddedToSystemUserDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserId,
    Guid UserIdentityId
) : DomainEvent(Id, OccurredAt);