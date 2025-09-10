using ProperTea.Application.Shared;
using ProperTea.Cqrs;
using ProperTea.UserManagement.Domain.SystemUsers;

namespace ProperTea.UserManagement.Application.Commands;

public record UpdateUserProfileCommand(
    Guid UserId,
    string FullName
) : ICommand;

public class UpdateUserProfileCommandHandler(
    ISystemUsersDomainService domainService,
    IUnitOfWork unitOfWork)
    : CommandHandler<UpdateUserProfileCommand>
{
    public override async Task HandleAsync(UpdateUserProfileCommand command, CancellationToken ct = default)
    {
        await domainService.UpdateUserProfile(command.UserId, command.FullName, ct);
        await unitOfWork.SaveAsync(ct);
    }
}