using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public interface IPairingService
{
    public Task<Result<Pair>> CreatePair(int breedingRabbitId, int maleRabbitId);
    public Task<Result<PaginatedResult<Pair>>> GetPaginatedPairs(PaginatedRequest<PairFilterDto> request);
    public Task<Result<Pair>> GetPairById(int pairId);
    public Task<Result<Pair>> UpdatePairingStatus(int pairId, PairingStatus pairingStatus);
}
