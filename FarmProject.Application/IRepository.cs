using FarmProject.Domain.Models;

namespace FarmProject.Application;

public interface IRepository<T> where T : Entity
{
    public T Create(T item);
    public T? GetById(int id);
    public List<T> GetAll();
    public T Update(T item);
    public int GetLastId();
}
