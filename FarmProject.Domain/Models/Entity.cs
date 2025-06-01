
using FarmProject.Domain.Events;

namespace FarmProject.Domain.Models;

public abstract class Entity()
{
    public int Id { get; set; }
    public List<DomainEvent> Events { get; set; } = [];
}
