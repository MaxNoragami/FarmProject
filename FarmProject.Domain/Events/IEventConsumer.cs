using FarmProject.Domain.Common;

namespace FarmProject.Domain.Events;

public interface IEventConsumer<T> where T : DomainEvent
{
    public Task<Result> ConsumeAsync(T domainEvent);
}
