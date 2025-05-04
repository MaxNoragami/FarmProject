using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public class PairingService(IRepository<Pair> pairingRepository, IRabbitService animalService) : IPairingService
{
    private readonly IRepository<Pair> _pairingRepository = pairingRepository;
    private readonly IRabbitService _animalService = animalService;

    public async Task<Pair> CreatePair(int firstAnimalId, int secondAnimalId)
    {
        var firstAnimal = await _animalService.GetRabbitById(firstAnimalId);
        var secondAnimal = await _animalService.GetRabbitById(secondAnimalId);

        if (!ValidatePair(firstAnimal, secondAnimal))
            throw new ArgumentException("Those animals are not able to be paired.");

        firstAnimal = await _animalService.UpdateBreedingStatus(firstAnimal.Id, BreedingStatus.Paired);
        secondAnimal = await _animalService.UpdateBreedingStatus(secondAnimal.Id, BreedingStatus.Paired);

        var requestPair = new Pair()
        {
            Id = GetNextId(),
            MaleId = (firstAnimal.Gender == Gender.Male)? firstAnimal.Id : secondAnimal.Id,
            FemaleId = (firstAnimal.Gender == Gender.Female) ? firstAnimal.Id : secondAnimal.Id,
            StartDate = DateTime.Now,
            PairingStatus = PairingStatus.Active
        };

        var createdPair = _pairingRepository.Create(requestPair);

        return createdPair;
    }

    public Task<List<Pair>> GetAllPairs()
    {
        var requestPairs = _pairingRepository.GetAll();
        return Task.FromResult(requestPairs);
    }

    public Task<Pair> GetPairById(int pairId)
    {
        var requestPair = _pairingRepository.GetById(pairId)
            ?? throw new ArgumentException("Pair not found.");

        return Task.FromResult(requestPair);
    }

    public Task<Pair> UpdatePairingStatus(int pairId, PairingStatus pairingStatus)
    {
        var requestPair = _pairingRepository.GetById(pairId)
            ?? throw new ArgumentException("Pair not found.");

        requestPair.PairingStatus = pairingStatus;

        if(pairingStatus != PairingStatus.Active)
        {
            requestPair.EndDate = DateTime.Now;

            if (pairingStatus == PairingStatus.Successful)
                _ = _animalService.UpdateBreedingStatus(requestPair.FemaleId, 
                                                        BreedingStatus.Pregnant);
            else
                _ = _animalService.UpdateBreedingStatus(requestPair.FemaleId,
                                                        BreedingStatus.Available);

            _ = _animalService.UpdateBreedingStatus(requestPair.MaleId,
                                                        BreedingStatus.Available);
        }

        var updatedPair = _pairingRepository.Update(requestPair);
        return Task.FromResult(updatedPair);
    }

    private bool ValidatePair(Rabbit firstAnimal, Rabbit secondAnimal)
    {
        if(firstAnimal.Gender != secondAnimal.Gender && 
            firstAnimal.BreedingStatus == BreedingStatus.Available &&
            secondAnimal.BreedingStatus == BreedingStatus.Available)
        {
            return true;
        }

        return false;
    }

    private int GetNextId()
        => _pairingRepository.GetLastId();
}
