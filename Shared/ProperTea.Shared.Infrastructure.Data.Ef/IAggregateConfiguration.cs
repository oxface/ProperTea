using ProperTea.Shared.Domain;

namespace ProperTea.Shared.Infrastructure.Data.Ef;

public interface IAggregateConfiguration<in T> where T : EntityBase
{
    void ConfigureIncludes(IQueryable<T> query);
}