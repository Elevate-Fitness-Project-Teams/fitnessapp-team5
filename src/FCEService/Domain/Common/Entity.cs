using System.ComponentModel.DataAnnotations.Schema;

namespace FCEService.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; }

    private readonly List<DomainEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity()
    { }

    protected Entity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }
    //we make add domin eve nt public because we want to be able to add domain events from outside the entity, 
    //for example from the application layer when we want to raise an event after a command is executed.
    //This allows us to decouple the domain events from the entity and make it easier to manage them in a
    //centralized way, such as using a domain event dispatcher or mediator.
    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}