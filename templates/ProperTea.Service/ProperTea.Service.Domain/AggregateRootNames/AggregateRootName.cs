using ProperTea.Shared.Domain;
using ProperTea.Shared.Domain.Events;

namespace ProperTea.Service.Domain.AggregateRootNames;

public class AggregateRootName : AggregateRoot
{
    private AggregateRootName() : base(Guid.Empty)
    {
    } // For deserialization

    private AggregateRootName(Guid id) : base(id)
    {
        Id = id;

        RaiseDomainEvent(new AggregateRootNameCreatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id));
    }

    public static AggregateRootName Create()
    {
        return new AggregateRootName(Guid.NewGuid());
    }
}

public record AggregateRootNameCreatedDomainEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid AggregateRootNameId
) : DomainEvent(EventId, OccurredAt);