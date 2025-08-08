using ProperTea.Shared.Application.Commands;
using ProperTea.SystemOwner.Domain.SystemOwner;

namespace ProperTea.SystemOwner.Application.SystemOwner.Commands;

public class CreateSystemOwnerCommandHandler(ISystemOwnerDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateSystemOwnerCommand, Guid>
{
    public async Task<Guid> HandleAsync(CreateSystemOwnerCommand command)
    {
        //TODO:
        var systemOwner = await domainService.CreateSystemOwnerAsync(command.Name);
        await unitOfWork.SaveChangesAsync();
        return systemOwner.Id;
    }
}