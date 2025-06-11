using FarmProject.Application.Events;
using FarmProject.Domain.Events;

namespace FarmProject.Application.UnitTests.Mocks;

public class MockDomainEventDispatcher : DomainEventDispatcher
{
    public List<DomainEvent> DispatchedEvents { get; } = new List<DomainEvent>();

    public MockDomainEventDispatcher() : base(null!) { }

    public new Task DispatchEventsAsync(IEnumerable<DomainEvent> domainEvents)
    {
        DispatchedEvents.AddRange(domainEvents);
        return Task.CompletedTask;
    }
}