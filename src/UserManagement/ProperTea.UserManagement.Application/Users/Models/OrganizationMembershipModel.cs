namespace ProperTea.UserManagement.Application.Users.Models;

public record OrganizationMembershipModel(
    Guid OrganizationId,
    string Role,
    DateTime JoinedAt
);