using ProperTea.Company.Domain;
using ProperTea.Shared.Application.Commands;

namespace ProperTea.Company.Application.Commands;

public class DeleteCompanyCommandHandler(ICompanyDomainService domainService, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteCompanyCommand>
{
    public async Task<object?> HandleAsync(DeleteCompanyCommand command)
    {
        await domainService.DeleteCompanyAsync(command.Id);
        await unitOfWork.SaveChangesAsync();
        return null;
    }
}