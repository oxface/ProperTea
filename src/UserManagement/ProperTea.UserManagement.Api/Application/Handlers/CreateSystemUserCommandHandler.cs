using ProperTea.Cqrs;
using ProperTea.UserManagement.Api.Application.Commands;
using ProperTea.UserManagement.Api.Domain.Users;

namespace ProperTea.UserManagement.Api.Application.Handlers;

public class CreateSystemUserCommandHandler : ICommandHandler<CreateSystemUserCommand>
{
    private readonly ISystemUserRepository _userRepository;

    public CreateSystemUserCommandHandler(ISystemUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(CreateSystemUserCommand command, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingUser != null)
            throw new InvalidOperationException($"User with email '{command.Email}' already exists");

        var user = SystemUser.Create(command.Email, command.FullName);
        await _userRepository.SaveAsync(user, cancellationToken);
    }
}