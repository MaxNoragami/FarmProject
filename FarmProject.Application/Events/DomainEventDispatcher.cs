using FarmProject.Domain.Events;
using Microsoft.Extensions.DependencyInjection;


namespace FarmProject.Application.Events;

public class DomainEventDispatcher(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task DispatchEventsAsync(IEnumerable<DomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
            await DispatchEventAsync(domainEvent);
    }

    private async Task DispatchEventAsync(DomainEvent domainEvent)
    {
        var eventType = domainEvent.GetType();
        var consumerType = typeof(IEventConsumer<>).MakeGenericType(eventType);

        var consumers = _serviceProvider.GetServices(consumerType);

        foreach(var consumer in consumers)
        {
            var method = consumerType.GetMethod("ConsumeAsync");
            await (Task)method.Invoke(consumer, [domainEvent]);
        }    
    }
}
