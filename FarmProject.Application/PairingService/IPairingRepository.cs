using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public interface IPairingRepository
{
    public Task<Pair> AddAsync(Pair pair);
    public Task<Pair?> GetByIdAsync(int pairId);
    public Task<PaginatedResult<Pair>> GetPaginatedAsync(PaginatedRequest<PairFilterDto> request);
    public Task<Pair> UpdateAsync(Pair pair);
    public Task RemoveAsync(Pair pair);
    public Task<Pair?> GetMostRecentPairByBreedingRabbitIdsAsync(int breedingRabbitId, int maleRabbitId);

}
