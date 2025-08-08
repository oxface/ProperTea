using ProperTea.Shared.Application.Commands;
using ProperTea.SystemOwner.Domain.SystemOwner;

namespace ProperTea.SystemOwner.Application.SystemOwner.Commands;

public class ChangeSystemOwnerNameCommandHandler(ISystemOwnerDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<ChangeSystemOwnerNameCommand>
{
    public async Task<object?> HandleAsync(ChangeSystemOwnerNameCommand command)
    {
        await domainService.ChangeSystemOwnerNameAsync(command.Id, command.NewName);
        await unitOfWork.SaveChangesAsync();
        return null;
    }
}