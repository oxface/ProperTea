using ProperTea.Shared.Application.Commands;
using ProperTea.SystemUser.Domain;

namespace ProperTea.SystemUser.Application.Commands;

public class DeleteSystemUserCommandHandler(ISystemUserDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteSystemUserCommand>
{
    public async Task<object?> HandleAsync(DeleteSystemUserCommand command)
    {
        await domainService.DeleteSystemUserAsync(command.Id);
        await unitOfWork.SaveChangesAsync();
        return null;
    }
}