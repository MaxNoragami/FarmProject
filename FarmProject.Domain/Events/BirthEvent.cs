namespace FarmProject.Domain.Events;

public class BirthEvent : DomainEvent
{
    public int BreedingRabbitId { get; set; }
    public int OffspringCount { get; set; }
    public DateTime BirthDate { get; set; }
}

