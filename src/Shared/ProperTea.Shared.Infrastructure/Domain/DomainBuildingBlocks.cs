using ProperTea.Domain.Shared.Events;

namespace ProperTea.Shared.Infrastructure.Domain;

public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = [];

    public Guid Id { get; protected init; }

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public abstract class Entity
{
    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; protected init; }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other || GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public abstract record ValueObject;