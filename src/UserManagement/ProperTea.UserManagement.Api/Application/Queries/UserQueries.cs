using ProperTea.Contracts.CQRS;

namespace ProperTea.UserManagement.Api.Application.Queries;

public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto?>;

public record GetUserByEmailQuery(string Email) : IQuery<UserDto?>;

public record CheckUserExistsQuery(string Email) : IQuery<bool>;

public record UserDto(
    Guid Id,
    string Email,
    string FullName,
    DateTime CreatedAt,
    bool IsActive,
    IEnumerable<OrganizationMembershipDto> Organizations
);

public record OrganizationMembershipDto(
    Guid OrganizationId,
    string Role,
    DateTime JoinedAt
);
