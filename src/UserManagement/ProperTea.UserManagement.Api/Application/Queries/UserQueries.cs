using ProperTea.Contracts.CQRS;

namespace ProperTea.UserManagement.Api.Application.Queries;

public record GetUserByIdQuery(Guid UserId) : IQuery<SystemUserModel?>;

public record GetUserByEmailQuery(string Email) : IQuery<SystemUserModel?>;

public record CheckUserExistsQuery(string Email) : IQuery<bool>;

public record SystemUserModel(
    Guid Id,
    string Email,
    string FullName,
    DateTime CreatedAt,
    bool IsActive,
    IEnumerable<OrganizationMembershipModel> Organizations
);

public record OrganizationMembershipModel(
    Guid OrganizationId,
    string Role,
    DateTime JoinedAt
);
