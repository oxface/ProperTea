using ProperTea.Shared.Domain;

namespace ProperTea.UserManagement.Domain.Users;

public interface IUserRepository : IRepository<User, UserFilter>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsAsync(string email, CancellationToken ct = default);
}

public record UserFilter
{
    public string FullName { get; init; } = null!;
}