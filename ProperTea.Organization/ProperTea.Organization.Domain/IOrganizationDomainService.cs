namespace ProperTea.Organization.Domain;

public interface IOrganizationDomainService : IDomainService
{
    Task<Organization> CreateOrganizationAsync(string name, CancellationToken ct = default);
    Task DeleteOrganizationAsync(Guid id, CancellationToken ct = default);
    Task ChangeOrganizationNameAsync(Guid id, string newName, CancellationToken ct = default);
}