namespace ProperTea.Company.Domain;

public interface ICompanyDomainService : IDomainService
{
    Task<Company> CreateCompanyAsync(string name, Guid systemOwnerId, CancellationToken ct = default);
    Task DeleteCompanyAsync(Guid id, CancellationToken ct = default);
    Task ChangeCompanyNameAsync(Guid id, string newName, CancellationToken ct = default);
}