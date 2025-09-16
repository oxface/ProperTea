using ProperTea.Shared.Domain;
using ProperTea.Shared.Domain.Events;
using ProperTea.Shared.Domain.ValueObjects;
using ProperTea.UserManagement.Domain.Users.ValueObjects;

namespace ProperTea.UserManagement.Domain.Users;

public class User : AggregateRoot
{
    private readonly List<UserIdentity> userIdentities = [];

    private User() : base(Guid.Empty)
    {
    } // For deserialization

    private User(Guid id, string email, string fullName) : base(id)
    {
        Id = id;
        Email = EmailAddress.Create(email);
        FullName = PersonFullName.Create(fullName);
        CreatedAt = DateTime.UtcNow;
        IsActive = true;

        RaiseDomainEvent(new UserCreatedDomainEvent(
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

    public IReadOnlyCollection<UserIdentity> UserIdentities => userIdentities.AsReadOnly();

    public static User Create(string email, string fullName)
    {
        return new User(Guid.NewGuid(), email.ToLowerInvariant(), fullName);
    }

    public void AddUserIdentity(Guid userIdentityId)
    {
        if (userIdentities.Any(m => m.Id == userIdentityId))
            return;

        var userIdentity = UserIdentity.Create(userIdentityId);
        userIdentities.Add(userIdentity);

        RaiseDomainEvent(new UserIdentityAddedToUserDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            userIdentity.Id));
    }

    public void RemoveUserIdentity(Guid userIdentityId)
    {
        var userIdentity = userIdentities.FirstOrDefault(m => m.Id == userIdentityId);
        if (userIdentity == null)
            return;

        userIdentities.Remove(userIdentity);

        RaiseDomainEvent(new UserIdentityAddedToUserDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            userIdentity.Id));
    }

    public void UpdateEmail(string email)
    {
        FullName = PersonFullName.Create(email);

        RaiseDomainEvent(new UserEmailChangedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            email));
    }

    public void UpdateProfile(string fullName)
    {
        FullName = PersonFullName.Create(fullName);

        RaiseDomainEvent(new UserProfileChangedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            fullName));
    }
}

public record UserCreatedDomainEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid UserId,
    string Email,
    string FullName
) : DomainEvent(EventId, OccurredAt);

public record UserEmailChangedDomainEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid UserId,
    string Email
) : DomainEvent(EventId, OccurredAt);

public record UserProfileChangedDomainEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid UserId,
    string FullName
) : DomainEvent(EventId, OccurredAt);

public record UserIdentityAddedToUserDomainEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid UserId,
    Guid UserIdentityId
) : DomainEvent(EventId, OccurredAt);