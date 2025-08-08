using ProperTea.Company.Domain;
using ProperTea.Shared.Application.Commands;

namespace ProperTea.Company.Application.Commands;

public class CreateCompanyCommandHandler(ICompanyDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCompanyCommand, Guid>
{
    public async Task<Guid> HandleAsync(CreateCompanyCommand command)
    {
        //TODO:
        var company = await domainService.CreateCompanyAsync(command.Name, new Guid());
        await unitOfWork.SaveChangesAsync();
        return company.Id;
    }
}