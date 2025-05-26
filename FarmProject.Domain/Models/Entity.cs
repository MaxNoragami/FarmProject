namespace FarmProject.Domain.Models;

public abstract class Entity(int id)
{
    public int Id { get; set; } = id;
    public List<FarmEvent> FarmEvents { get; private set; } = [];
}
