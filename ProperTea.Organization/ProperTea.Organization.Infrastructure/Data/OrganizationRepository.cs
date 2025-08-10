using Microsoft.EntityFrameworkCore;

using ProperTea.Organization.Domain;
using ProperTea.Shared.Infrastructure.Data.Ef;

namespace ProperTea.Organization.Infrastructure.Data;

public class OrganizationRepository(OrganizationDbContext context)
    : RepositoryBase<Domain.Organization, OrganizationFilter>(context), IOrganizationRepository
{
    protected override IAggregateConfiguration<Domain.Organization>? AggregateConfiguration =>
        new OrganizationAggregateConfiguration();

    protected override IQueryable<Domain.Organization> ApplyFilter(
        IQueryable<Domain.Organization> query,
        OrganizationFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(i => i.Name.Value.Contains(filter.Name));

        return query;
    }
}