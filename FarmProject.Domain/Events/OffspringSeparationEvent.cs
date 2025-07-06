using FarmProject.Domain.Constants;

namespace FarmProject.Domain.Events;

public class OffspringSeparationEvent : DomainEvent
{
    public DateTime CreatedOn { get; set; }
    public int NewCageId { get; set; }
}
