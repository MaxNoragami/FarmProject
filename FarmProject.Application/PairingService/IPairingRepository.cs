using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public interface IPairingRepository
{
    public Task<Pair> AddAsync(Pair pair);
    public Task<Pair?> GetByIdAsync(int pairId);
    public Task<List<Pair>> GetAllAsync();
    public Task<Pair> UpdateAsync(Pair pair);
    public Task RemoveAsync(Pair pair);
    public Task<Pair?> GetMostRecentPairByBreedingRabbitIdsAsync(int breedingRabbitId1, int breedingRabbitId2);

}
