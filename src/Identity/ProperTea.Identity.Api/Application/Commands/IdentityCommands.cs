using ProperTea.Contracts.CQRS;

namespace ProperTea.Identity.Api.Application.Commands;

public record CreateIdentityCommand(
    Guid UserId,
    string Email,
    string Password
) : ICommand;

public record AuthenticateUserCommand(
    string Email,
    string Password
) : ICommand;

public record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword
) : ICommand;
