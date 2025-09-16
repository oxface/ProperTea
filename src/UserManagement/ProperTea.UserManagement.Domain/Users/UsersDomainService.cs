using ProperTea.Shared.Domain;
using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.UserManagement.Domain.Users;

public interface IUserDomainService : IDomainService
{
    Task CreateUser(
        string email,
        string fullName,
        CancellationToken ct = default);

    Task UpdateUserProfile(Guid userId, string fullName, CancellationToken ct = default);

    Task AddUserIdentity(
        Guid userId,
        Guid userIdentityId,
        CancellationToken ct = default);
}

public class UserDomainService(IUserRepository userRepository) : IUserDomainService
{
    public async Task CreateUser(
        string email,
        string fullName,
        CancellationToken ct = default)
    {
        var existingUser = await userRepository.GetByEmailAsync(email, ct);
        if (existingUser != null)
            throw new DomainException("User.AlreadyExists");

        var user = User.Create(email, fullName);

        await userRepository.AddAsync(user, ct);
    }

    public async Task UpdateUserProfile(Guid userId, string fullName, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user == null)
            throw new DomainException("User.DoesNotExist");

        user.UpdateProfile(fullName);

        await userRepository.UpdateAsync(user, ct);
    }

    public async Task AddUserIdentity(
        Guid userId,
        Guid userIdentityId,
        CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user == null)
            throw new DomainException("User.DoesNotExist");

        user.AddUserIdentity(userIdentityId);

        await userRepository.UpdateAsync(user, ct);
    }
}