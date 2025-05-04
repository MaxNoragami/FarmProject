using FarmProject.Domain.Models;

namespace FarmProject.Application.RabbitsService;

public interface IRabbitRepository
{
    public Rabbit Create(Rabbit rabbit);
    public Rabbit? GetById(int id);
    public List<Rabbit> GetAll();
    public int GetLastId();
}
