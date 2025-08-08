using ProperTea.Company.Domain.DomainEvents;
using ProperTea.Shared.Domain.DomainEvents;
using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.Company.Domain;

public class CompanyDomainService(ICompanyRepository repository, IDomainEventDispatcher eventDispatcher)
    : DomainServiceBase, ICompanyDomainService
{
    public async Task<Company> CreateCompanyAsync(string name,
        Guid systemOwnerId,
        CancellationToken ct = default)
    {
        var company = Company.Create(name, systemOwnerId);
        await repository.AddAsync(company, ct);
        return company;
    }

    public async Task DeleteCompanyAsync(Guid id, CancellationToken ct = default)
    {
        var company = await repository.GetByIdAsync(id, ct);
        if (company is null)
            throw new EntityNotFoundException(nameof(Company), id);

        if (!company.AllowDelete())
            throw new InvalidOperationException("Company cannot be deleted");
        await repository.DeleteAsync(company, ct);

        eventDispatcher.Enqueue(new CompanyDeletedDomainEvent(company.Id));
    }


    public async Task ChangeCompanyNameAsync(Guid id, string newName, CancellationToken ct = default)
    {
        var company = await repository.GetByIdAsync(id, ct);
        if (company == null)
            throw new EntityNotFoundException(nameof(Company), id);

        company.ChangeName(newName);
    }
}