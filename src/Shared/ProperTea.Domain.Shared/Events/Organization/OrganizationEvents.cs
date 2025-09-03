namespace ProperTea.Contracts.Events.Organization;

public record OrganizationCreatedEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid OrganizationId,
    string Name,
    string AdminEmail,
    string AdminFullName,
    bool IsNewUser
) : IntegrationEvent(Id, OccurredAt);

public record UserCreatedEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserId,
    string Email,
    string FullName,
    Guid OrganizationId
) : IntegrationEvent(Id, OccurredAt);

public record IdentityCreatedEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserId,
    string Email
) : IntegrationEvent(Id, OccurredAt);
