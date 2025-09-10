namespace ProperTea.Application.Shared;

public interface IUnitOfWork
{
    Task<int> SaveAsync(CancellationToken ct = default);
}