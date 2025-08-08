namespace ProperTea.SystemUser.Domain;

public interface ISystemUserDomainService : IDomainService
{
    Task<SystemUser> CreateSystemUserAsync(string name, CancellationToken ct = default);
    Task DeleteSystemUserAsync(Guid id, CancellationToken ct = default);
    Task ChangeSystemUserNameAsync(Guid id, string newName, CancellationToken ct = default);
}