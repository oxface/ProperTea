using ProperTea.ProperCqrs;
using ProperTea.Shared.Application;
using ProperTea.UserManagement.Domain.Users;

namespace ProperTea.UserManagement.Application.Users.Commands;

public record UpdateUserProfileCommand(
    Guid UserId,
    string FullName
) : ICommand;

public class UpdateUserProfileCommandHandler(
    IUserDomainService domainService,
    IUnitOfWork unitOfWork)
    : CommandHandler<UpdateUserProfileCommand>
{
    public override async Task HandleAsync(UpdateUserProfileCommand command, CancellationToken ct = default)
    {
        await domainService.UpdateUserProfile(command.UserId, command.FullName, ct);
        await unitOfWork.SaveAsync(ct);
    }
}