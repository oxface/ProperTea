namespace ProperTea.Contracts.DTOs.Organization;

public record CreateOrganizationRequest(
    string Name,
    string Description,
    string AdminEmail,
    string AdminFullName,
    string AdminPassword,
    bool IsExistingUser = false
);

public record CreateOrganizationResponse(
    Guid OrganizationId,
    string Name,
    Guid AdminUserId,
    string AdminEmail,
    bool UserCreated
);

public record OrganizationDto(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    bool IsActive
);
