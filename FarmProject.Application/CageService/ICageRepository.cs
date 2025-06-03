using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.CageService;

public interface ICageRepository
{
    public Task<Cage> AddAsync(Cage cage);
    public Task<Cage?> GetByIdAsync(int cageId);
    public Task<List<Cage>> GetAllAsync();
    public Task<List<Cage>> FindAsync(ISpecification<Cage> specification);
    public Task<Cage> UpdateAsync(Cage cage);
    public Task RemoveAsync(Cage cage);
}
