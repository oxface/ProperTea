using ProperTea.Application.Shared;
using ProperTea.Cqrs;
using ProperTea.UserManagement.Domain.SystemUsers;

namespace ProperTea.UserManagement.Application.Commands;

public record AddUserToOrganizationCommand(
    Guid UserId,
    Guid OrganizationId,
    SystemUserOrganizationRole OrganizationRole
) : ICommand;

public class AddUserToOrganizationCommandHandler(
    ISystemUsersDomainService domainService,
    IUnitOfWork unitOfWork)
    : CommandHandler<AddUserToOrganizationCommand>
{
    public override async Task HandleAsync(AddUserToOrganizationCommand command, CancellationToken ct = default)
    {
        await domainService.AddUserToOrganizationAsync(
            command.UserId,
            command.OrganizationId,
            command.OrganizationRole,
            ct);
        await unitOfWork.SaveAsync(ct);
    }
}