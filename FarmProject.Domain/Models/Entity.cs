
using FarmProject.Domain.Events;

namespace FarmProject.Domain.Models;

public abstract class Entity()
{
    private readonly List<DomainEvent> _domainEvents = [];

    public int Id { get; set; }
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}
