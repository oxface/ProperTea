using ProperTea.Cqrs;
using ProperTea.UserManagement.Api.Application.Commands;
using ProperTea.UserManagement.Api.Domain.Users;

namespace ProperTea.UserManagement.Api.Application.Handlers;

public class AddUserToOrganizationCommandHandler : ICommandHandler<AddUserToOrganizationCommand>
{
    private readonly ISystemUserRepository _userRepository;

    public AddUserToOrganizationCommandHandler(ISystemUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(AddUserToOrganizationCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user == null) throw new InvalidOperationException($"User with ID '{command.UserId}' not found");

        user.AddOrganizationMembership(command.OrganizationId, command.Role);
        await _userRepository.SaveAsync(user, cancellationToken);
    }
}