using FarmProject.Application;
using FarmProject.Domain.Models;

namespace FarmProject.Infrastructure;

public class InMemoryRepository<T> : IRepository<T> where T : Entity
{
    private readonly List<T> _items = new();

    public T Create(T item)
    {
        _items.Add(item);
        return item;
    }

    public List<T> GetAll()
        => _items;

    public T? GetById(int id)
        => _items.SingleOrDefault(i => i.Id == id);

    public int GetLastId()
        => _items.Any()
            ? _items.Max(i => i.Id) + 1
            : 1;

    public T Update(T item)
    {
        var index = _items.FindIndex(i => i.Id == item.Id);

        if (index < 0)
            throw new ArgumentException($"{typeof(T)} could not be updated");

        _items[index] = item;

        return item;
    }
}
