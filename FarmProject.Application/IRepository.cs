using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application;

public interface IRepository<T> where T : Entity
{
    public T Create(T item);
    public T? GetById(int id);
    public List<T> GetAll();
    public List<T> Find(ISpecification<T> specification);
    public T Update(T item);
    public int GetLastId();
}
