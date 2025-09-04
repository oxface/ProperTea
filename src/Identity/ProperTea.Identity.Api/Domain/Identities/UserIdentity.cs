using ProperTea.Domain.Shared.Events;
using ProperTea.Shared.Infrastructure.Domain;

namespace ProperTea.Identity.Api.Domain.Identities;

public class UserIdentity : AggregateRoot
{
    private UserIdentity()
    {
    } // For deserialization

    private UserIdentity(Guid id, Guid userId, string email, string passwordHash)
    {
        Id = id;
        UserId = userId;
        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;

        RaiseDomainEvent(new IdentityCreatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            UserId,
            Email));
    }

    public Guid UserId { get; }
    public string Email { get; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public bool IsActive { get; private set; }

    public static UserIdentity Create(Guid userId, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

        return new UserIdentity(Guid.NewGuid(), userId, email, passwordHash);
    }

    public bool VerifyPassword(string providedPasswordHash)
    {
        return PasswordHash == providedPasswordHash;
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("New password hash cannot be empty", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;

        RaiseDomainEvent(new PasswordChangedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            UserId));
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}

public record IdentityCreatedDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid IdentityId,
    Guid UserId,
    string Email
) : DomainEvent(Id, OccurredAt);

public record PasswordChangedDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid IdentityId,
    Guid UserId
) : DomainEvent(Id, OccurredAt);