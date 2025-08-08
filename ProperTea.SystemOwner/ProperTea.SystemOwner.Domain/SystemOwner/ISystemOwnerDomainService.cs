namespace ProperTea.SystemOwner.Domain.SystemOwner;

public interface ISystemOwnerDomainService : IDomainService
{
    Task<SystemOwner> CreateSystemOwnerAsync(string name, CancellationToken ct = default);
    Task DeleteSystemOwnerAsync(Guid id, CancellationToken ct = default);
    Task ChangeSystemOwnerNameAsync(Guid id, string newName, CancellationToken ct = default);
}