namespace FarmProject.Domain.Models;

public abstract class Entity(int id)
{
    public int Id { get; set; } = id;
}
