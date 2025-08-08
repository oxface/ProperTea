using ProperTea.Organization.Domain;
using ProperTea.Shared.Application.Commands;

namespace ProperTea.Organization.Application.Commands;

public class CreateOrganizationCommandHandler(IOrganizationDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateOrganizationCommand, Guid>
{
    public async Task<Guid> HandleAsync(CreateOrganizationCommand command)
    {
        //TODO:
        var organization = await domainService.CreateOrganizationAsync(command.Name);
        await unitOfWork.SaveChangesAsync();
        return organization.Id;
    }
}