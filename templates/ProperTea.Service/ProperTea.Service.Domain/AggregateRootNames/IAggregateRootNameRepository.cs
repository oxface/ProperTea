using ProperTea.Shared.Domain;

namespace ProperTea.Service.Domain.AggregateRootNames;

public interface IAggregateRootNameRepository : IRepository<AggregateRootName, AggregateRootNameFilter>
{
}

public record AggregateRootNameFilter
{
}