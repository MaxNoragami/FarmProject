namespace FarmProject.Domain.Events;

public class SacrificationCreatedEvent : DomainEvent
{
    public int CageId { get; set; }
    public int Amount { get; set; }
    public int OrderRequestId { get; set; }
    public DateTime CreatedOn { get; set; }
}
