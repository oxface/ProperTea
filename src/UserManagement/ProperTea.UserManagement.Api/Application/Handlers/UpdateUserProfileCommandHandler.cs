using ProperTea.Contracts.CQRS;
using ProperTea.UserManagement.Api.Application.Commands;
using ProperTea.UserManagement.Api.Domain.Users;

namespace ProperTea.UserManagement.Api.Application.Handlers;

public class UpdateUserProfileCommandHandler : ICommandHandler<UpdateUserProfileCommand>
{
    private readonly ISystemUserRepository _userRepository;

    public UpdateUserProfileCommandHandler(ISystemUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(UpdateUserProfileCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID '{command.UserId}' not found");
        }

        user.UpdateProfile(command.FullName);
        await _userRepository.SaveAsync(user, cancellationToken);
    }
}
