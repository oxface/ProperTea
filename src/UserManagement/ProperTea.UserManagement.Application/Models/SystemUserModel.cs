using ProperTea.UserManagement.Domain.SystemUsers;

namespace ProperTea.UserManagement.Application.Models;

public record SystemUserModel(
    Guid Id,
    string Email,
    string FullName,
    DateTime CreatedAt,
    bool IsActive,
    IEnumerable<OrganizationMembershipModel> Organizations
)
{
    public static SystemUserModel FromEntity(SystemUser user)
    {
        return new SystemUserModel(
            user.Id,
            user.Email,
            user.FullName,
            user.CreatedAt,
            user.IsActive,
            user.OrganizationMemberships.Select(m => new OrganizationMembershipModel(
                m.OrganizationId,
                m.OrganizationRole.ToString(),
                m.JoinedAt)));
    }
}

public record OrganizationMembershipModel(
    Guid OrganizationId,
    string Role,
    DateTime JoinedAt
);