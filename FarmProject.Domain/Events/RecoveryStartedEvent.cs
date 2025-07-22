namespace FarmProject.Domain.Events;

public class RecoveryStartedEvent : DomainEvent
{
    public int BreedingRabbitId { get; set; }
    public DateTime RecoveryStartDate { get; set; }
}