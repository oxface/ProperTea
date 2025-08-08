using ProperTea.Shared.Application.Commands;
using ProperTea.SystemUser.Domain;

namespace ProperTea.SystemUser.Application.Commands;

public class CreateSystemUserCommandHandler(ISystemUserDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateSystemUserCommand, Guid>
{
    public async Task<Guid> HandleAsync(CreateSystemUserCommand command)
    {
        //TODO:
        var systemUser = await domainService.CreateSystemUserAsync(command.Name);
        await unitOfWork.SaveChangesAsync();
        return systemUser.Id;
    }
}