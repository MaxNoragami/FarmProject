using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application;

public interface IRepository<T> where T : Entity
{
    // Change all to ...Async()
    public T Create(T item); // Better called Add?
    public T? GetById(int id);
    public List<T> GetAll(); // Via pagination
    public List<T> Find(ISpecification<T> specification);
    public T Update(T item);
    public int GetLastId(); // Useless in case of DbContext
    // RemoveAsync() method declaration
}
