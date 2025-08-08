namespace ProperTea.SystemOwner.Domain;

public interface ISystemOwnerDomainService : IDomainService
{
    Task<SystemOwner> CreateSystemOwnerAsync(string name, CancellationToken ct = default);
    Task DeleteSystemOwnerAsync(Guid id, CancellationToken ct = default);
    Task ChangeSystemOwnerNameAsync(Guid id, string newName, CancellationToken ct = default);
}