using ProperTea.Infrastructure.Persistence;

namespace ProperTea.Organization.Api.Domain.Organizations;

public interface IOrganizationRepository : IRepository<Organization>
{
    Task<Organization?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
