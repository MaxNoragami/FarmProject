
namespace FarmProject.Domain.Models;

public abstract class Entity()
{
    public int Id { get; set; }
    public List<DomainEvent> Events { get; set; } = [];
}

public class DomainEvent;

public class BreedEvent : DomainEvent
{
    public DateTime StartDate { get; set; }
    public int[] RabbitIds { get; set; }
}
