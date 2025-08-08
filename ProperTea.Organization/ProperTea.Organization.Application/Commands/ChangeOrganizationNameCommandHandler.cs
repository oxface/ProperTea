using ProperTea.Organization.Domain;
using ProperTea.Shared.Application.Commands;

namespace ProperTea.Organization.Application.Commands;

public class ChangeOrganizationNameCommandHandler(IOrganizationDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<ChangeOrganizationNameCommand>
{
    public async Task<object?> HandleAsync(ChangeOrganizationNameCommand command)
    {
        await domainService.ChangeOrganizationNameAsync(command.Id, command.NewName);
        await unitOfWork.SaveChangesAsync();
        return null;
    }
}