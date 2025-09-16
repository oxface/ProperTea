using ProperTea.ProperCqrs;
using ProperTea.Shared.Application;
using ProperTea.UserManagement.Domain.Users;

namespace ProperTea.UserManagement.Application.Users.Commands;

public record CreateUserCommand(
    string Email,
    string FullName
) : ICommand;

public class CreateUserCommandHandler(
    IUserDomainService domainService,
    IUnitOfWork unitOfWork)
    : CommandHandler<CreateUserCommand>
{
    public override async Task HandleAsync(CreateUserCommand command, CancellationToken ct = default)
    {
        await domainService.CreateUser(command.Email, command.FullName, ct);
        await unitOfWork.SaveAsync(ct);
    }
}