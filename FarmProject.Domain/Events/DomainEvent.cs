namespace FarmProject.Domain.Events;

public class DomainEvent
{
    public DateTime OccuredOn { get; private set; } = DateTime.Now;
}
