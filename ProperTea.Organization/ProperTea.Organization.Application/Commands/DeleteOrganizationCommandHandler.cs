using ProperTea.Organization.Domain;
using ProperTea.Shared.Application.Commands;

namespace ProperTea.Organization.Application.Commands;

public class DeleteOrganizationCommandHandler(IOrganizationDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteOrganizationCommand>
{
    public async Task<object?> HandleAsync(DeleteOrganizationCommand command)
    {
        await domainService.DeleteOrganizationAsync(command.Id);
        await unitOfWork.SaveChangesAsync();
        return null;
    }
}