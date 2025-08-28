using Microsoft.Azure.Cosmos;
using ProperTea.Infrastructure.Cosmos;
using ProperTea.UserManagement.Api.Domain.Users;

namespace ProperTea.UserManagement.Api.Infrastructure.Persistence;

public class CosmosSystemUserRepository : CosmosRepository<SystemUser>, ISystemUserRepository
{
    public CosmosSystemUserRepository(Container container) : base(container)
    {
    }

    public async Task<SystemUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT * FROM c WHERE c.Email = '{email.ToLowerInvariant().Replace("'", "''")}'";
        var results = await QueryAsync(query, cancellationToken);
        return results.FirstOrDefault();
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await GetByEmailAsync(email, cancellationToken);
        return user != null;
    }

    public async Task<IEnumerable<SystemUser>> GetByOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT * FROM c JOIN m IN c.OrganizationMemberships WHERE m.OrganizationId = '{organizationId}'";
        return await QueryAsync(query, cancellationToken);
    }
}
