using ProperTea.Shared.Domain;

namespace ProperTea.Service.Domain.AggregateRootNames;

public interface IAggregateRootNameDomainService : IDomainService
{
}

public class AggregateRootNameDomainService(IAggregateRootNameRepository repository) : IAggregateRootNameDomainService
{
}