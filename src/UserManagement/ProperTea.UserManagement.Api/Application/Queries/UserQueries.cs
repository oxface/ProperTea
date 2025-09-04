using ProperTea.Cqrs;
using ProperTea.UserManagement.Api.Domain.Users;

namespace ProperTea.UserManagement.Api.Application.Queries;

public record GetUserByIdQuery(Guid UserId) : IQuery;

public record GetUserByEmailQuery(string Email) : IQuery;

public record CheckUserExistsQuery(string Email) : IQuery;

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
                m.Role.ToString(),
                m.JoinedAt)));
    }
}

public record OrganizationMembershipModel(
    Guid OrganizationId,
    string Role,
    DateTime JoinedAt
);