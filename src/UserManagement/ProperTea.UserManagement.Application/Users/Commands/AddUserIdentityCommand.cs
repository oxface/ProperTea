using ProperTea.ProperCqrs;
using ProperTea.Shared.Application;
using ProperTea.UserManagement.Domain.Users;

namespace ProperTea.UserManagement.Application.Users.Commands;

public record AddUserIdentityCommand(Guid UserId, Guid IdentityId) : ICommand
{
}

public class AddUserIdentityCommandHandler(
    IUserDomainService domainService,
    IUnitOfWork unitOfWork)
    : CommandHandler<AddUserIdentityCommand>
{
    public override async Task HandleAsync(AddUserIdentityCommand command, CancellationToken ct = default)
    {
        await domainService.AddUserIdentity(command.UserId, command.IdentityId, ct);
        await unitOfWork.SaveAsync(ct);
    }
}