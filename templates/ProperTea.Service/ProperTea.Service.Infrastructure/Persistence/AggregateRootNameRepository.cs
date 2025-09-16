using ProperTea.Service.Domain.AggregateRootNames;
using ProperTea.Shared.Infrastructure.Persistence;

namespace ProperTea.Service.Infrastructure.Persistence;

public class AggregateRootNameRepository(ServiceNameDbContext dbContext)
    : RepositoryBase<AggregateRootName, AggregateRootNameFilter>(dbContext), IAggregateRootNameRepository
{
    protected override IQueryable<AggregateRootName> ApplyFilter(IQueryable<AggregateRootName> query,
        AggregateRootNameFilter filter)
    {
        // TODO: implement filter
        return query;
    }
}