namespace FarmProject.Domain.Events;

public class BreedEvent : DomainEvent
{
    public int[] BreedingRabbitIds { get; set; }
    public DateTime StartDate { get; set; }
}
