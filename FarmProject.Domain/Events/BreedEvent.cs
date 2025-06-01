namespace FarmProject.Domain.Events;

public class BreedEvent : DomainEvent
{
    public int[] RabbitIds { get; set; }
    public DateTime StartDate { get; set; }
}
