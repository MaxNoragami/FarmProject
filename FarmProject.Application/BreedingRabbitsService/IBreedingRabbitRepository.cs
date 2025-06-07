using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.BreedingRabbitsService;

public interface IBreedingRabbitRepository
{
    public Task<BreedingRabbit> AddAsync(BreedingRabbit breedingRabbit);
    public Task<BreedingRabbit?> GetByIdAsync(int breedingRabbitId);
    public Task<List<BreedingRabbit>> GetAllAsync();
    public Task<List<BreedingRabbit>> FindAsync(ISpecification<BreedingRabbit> specification);
    public Task<BreedingRabbit> UpdateAsync(BreedingRabbit breedingRabbit);
    public Task RemoveAsync(BreedingRabbit breedingRabbit);
}
