using ProperTea.Shared.Application.Commands;
using ProperTea.SystemUser.Domain;

namespace ProperTea.SystemUser.Application.Commands;

public class ChangeSystemUserNameCommandHandler(ISystemUserDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<ChangeSystemUserNameCommand>
{
    public async Task<object?> HandleAsync(ChangeSystemUserNameCommand command)
    {
        await domainService.ChangeSystemUserNameAsync(command.Id, command.NewName);
        await unitOfWork.SaveChangesAsync();
        return null;
    }
}