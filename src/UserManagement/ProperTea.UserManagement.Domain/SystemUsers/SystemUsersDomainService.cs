using ProperTea.Domain.Shared;

namespace ProperTea.UserManagement.Domain.SystemUsers;

public interface ISystemUsersDomainService : IDomainService
{
    Task CreateSystemUser(
        string email,
        string fullName,
        CancellationToken ct = default);

    Task UpdateUserProfile(Guid userId, string fullName, CancellationToken ct = default);

    Task AddUserToOrganizationAsync(Guid userId,
        Guid organizationId,
        SystemUserOrganizationRole organizationRole,
        CancellationToken ct = default);
}

public class SystemUsersDomainService(ISystemUserRepository userRepository) : ISystemUsersDomainService
{
    public async Task CreateSystemUser(
        string email,
        string fullName,
        CancellationToken ct = default)
    {
        var existingUser = await userRepository.GetByEmailAsync(email, ct);
        if (existingUser != null)
            throw new InvalidOperationException($"User with email '{email}' already exists");

        var user = SystemUser.Create(email, fullName);

        await userRepository.AddAsync(user, ct);
    }

    public async Task AddUserToOrganizationAsync(Guid userId,
        Guid organizationId,
        SystemUserOrganizationRole organizationRole,
        CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user == null)
            throw new InvalidOperationException($"User with ID '{userId}' not found");

        user.AddOrganizationMembership(organizationId, organizationRole);

        await userRepository.UpdateAsync(user, ct);
    }

    public async Task UpdateUserProfile(Guid userId, string fullName, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user == null)
            throw new InvalidOperationException($"User with ID '{userId}' not found");

        user.UpdateProfile(fullName);

        await userRepository.UpdateAsync(user, ct);
    }
}