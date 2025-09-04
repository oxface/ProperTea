using ProperTea.Cqrs;
using ProperTea.UserManagement.Api.Domain.Users;

namespace ProperTea.UserManagement.Api.Application.Commands;

public record CreateSystemUserCommand(
    string Email,
    string FullName
) : ICommand;

public record AddUserToOrganizationCommand(
    Guid UserId,
    Guid OrganizationId,
    UserRole Role
) : ICommand;

public record UpdateUserProfileCommand(
    Guid UserId,
    string FullName
) : ICommand;