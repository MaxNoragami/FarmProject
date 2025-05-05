using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public interface IPairingService
{
    public Task<Result<Pair>> CreatePair(int firstAnimalId, int secondAnimalId);
    public Task<Result<List<Pair>>> GetAllPairs();
    public Task<Result<Pair>> GetPairById(int pairId);
    public Task<Result<Pair>> UpdatePairingStatus(int pairId, PairingStatus pairingStatus);
}
