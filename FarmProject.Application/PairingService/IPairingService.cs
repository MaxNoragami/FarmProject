using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public interface IPairingService
{
    public Task<Pair> CreatePair(int firstAnimalId, int secondAnimalId);
    public Task<List<Pair>> GetAllPairs();
    public Task<Pair> GetPairById(int pairId);
    public Task<Pair> UpdatePairingStatus(int pairId, PairingStatus pairingStatus);
}
