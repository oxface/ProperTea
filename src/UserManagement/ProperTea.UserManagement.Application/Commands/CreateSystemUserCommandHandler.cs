using ProperTea.Application.Shared;
using ProperTea.Cqrs;
using ProperTea.UserManagement.Domain.SystemUsers;

namespace ProperTea.UserManagement.Application.Commands;

public record CreateSystemUserCommand(
    string Email,
    string FullName
) : ICommand;

public class CreateSystemUserCommandHandler(
    ISystemUsersDomainService domainService,
    IUnitOfWork unitOfWork)
    : CommandHandler<CreateSystemUserCommand>
{
    public override async Task HandleAsync(CreateSystemUserCommand command, CancellationToken ct = default)
    {
        await domainService.CreateSystemUser(command.Email, command.FullName, ct);
        await unitOfWork.SaveAsync(ct);
    }
}