using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public interface IPairingService
{
    public Task<PairingProcess> CreatePair(int firstAnimalId, int secondAnimalId);
    public Task<List<PairingProcess>> GetAllPairs();
    public Task<PairingProcess> GetPairById(int pairId);
    public Task<PairingProcess> UpdatePairingStatus(int pairId, PairingStatus pairingStatus);
}
