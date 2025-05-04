using FarmProject.Application.Rabbits;
using FarmProject.Domain.Models;

namespace FarmProject.Infrastructure;

public class RabbitRepository : IRabbitRepository
{
    private readonly List<Rabbit> _rabbits = new();

    public Rabbit Create(Rabbit rabbit)
    {
        _rabbits.Add(rabbit);
        return rabbit;
    }

    public List<Rabbit> GetAll()
        => _rabbits;

    public Rabbit? GetById(int id)
        => _rabbits.SingleOrDefault(r => r.Id == id);

    public int GetLastId()
        => _rabbits.Any()
            ? _rabbits.Max(r => r.Id) + 1 
            : 1;
}
