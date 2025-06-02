namespace FarmProject.Domain.Events;

public class NestPrepEvent : DomainEvent
{
    public DateTime DueDate { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
}
