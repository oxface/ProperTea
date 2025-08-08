using ProperTea.Shared.Application.Commands;
using ProperTea.SystemOwner.Domain;

namespace ProperTea.SystemOwner.Application.Commands;

public class DeleteSystemOwnerCommandHandler(ISystemOwnerDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteSystemOwnerCommand>
{
    public async Task<object?> HandleAsync(DeleteSystemOwnerCommand command)
    {
        await domainService.DeleteSystemOwnerAsync(command.Id);
        await unitOfWork.SaveChangesAsync();
        return null;
    }
}