using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public class PairingService(IRepository<Pair> pairingRepository, IRabbitService animalService) : IPairingService
{
    private readonly IRepository<Pair> _pairingRepository = pairingRepository;
    private readonly IRabbitService _animalService = animalService;

    public async Task<Result<Pair>> CreatePair(int firstAnimalId, int secondAnimalId)
    {
        var firstAnimalResult = await _animalService.GetRabbitById(firstAnimalId);
        var secondAnimalResult = await _animalService.GetRabbitById(secondAnimalId);

        if (firstAnimalResult.IsFailure)
            return Result.Failure<Pair>(firstAnimalResult.Error);

        if (secondAnimalResult.IsFailure)
            return Result.Failure<Pair>(secondAnimalResult.Error);

        var firstAnimal = firstAnimalResult.Value;
        var secondAnimal = secondAnimalResult.Value;

        if (!PairingValidator.ValidatePair(firstAnimal, secondAnimal))
            return Result.Failure<Pair>(PairErrors.InvalidPairing);

        firstAnimalResult = await _animalService.UpdateBreedingStatus(firstAnimal.Id, BreedingStatus.Paired);
        secondAnimalResult = await _animalService.UpdateBreedingStatus(secondAnimal.Id, BreedingStatus.Paired);

        if (firstAnimalResult.IsFailure)
            return Result.Failure<Pair>(firstAnimalResult.Error);

        if (secondAnimalResult.IsFailure)
            return Result.Failure<Pair>(secondAnimalResult.Error);

        firstAnimal = firstAnimalResult.Value;
        secondAnimal = secondAnimalResult.Value;

        var requestPair = new Pair()
        {
            Id = GetNextId(),
            MaleId = (firstAnimal.Gender == Gender.Male)? firstAnimal.Id : secondAnimal.Id,
            FemaleId = (firstAnimal.Gender == Gender.Female) ? firstAnimal.Id : secondAnimal.Id,
            StartDate = DateTime.Now,
            PairingStatus = PairingStatus.Active
        };

        var createdPair = _pairingRepository.Create(requestPair);

        return Result.Success(createdPair);
    }

    public Task<Result<List<Pair>>> GetAllPairs()
    {
        var requestPairs = _pairingRepository.GetAll();
        return Task.FromResult(Result.Success(requestPairs));
    }

    public Task<Result<Pair>> GetPairById(int pairId)
    {
        var requestPair = _pairingRepository.GetById(pairId);

        if (requestPair == null)
            return Task.FromResult(Result.Failure<Pair>(PairErrors.NotFound));

        return Task.FromResult(Result.Success(requestPair));
    }

    public async Task<Result<Pair>> UpdatePairingStatus(int pairId, PairingStatus pairingStatus)
    {
        var requestPair = _pairingRepository.GetById(pairId);

        if (requestPair == null)
            return Result.Failure<Pair>(PairErrors.NotFound);

        requestPair.PairingStatus = pairingStatus;

        if(pairingStatus != PairingStatus.Active)
        {
            requestPair.EndDate = DateTime.Now;

            if (pairingStatus == PairingStatus.Successful)
            {
                var femaleResult = await _animalService.UpdateBreedingStatus(requestPair.FemaleId,
                                                        BreedingStatus.Pregnant);
                if (femaleResult.IsFailure)
                    return Result.Failure<Pair>(femaleResult.Error);
            }
            else
            {
                var femaleResult = await _animalService.UpdateBreedingStatus(requestPair.FemaleId,
                                                        BreedingStatus.Available);
                if (femaleResult.IsFailure)
                    return Result.Failure<Pair>(femaleResult.Error);
            }    

            var maleResult = await _animalService.UpdateBreedingStatus(requestPair.MaleId,
                                                        BreedingStatus.Available);
            if (maleResult.IsFailure)
                return Result.Failure<Pair>(maleResult.Error);
        }

        var updatedPair = _pairingRepository.Update(requestPair);
        return Result.Success(updatedPair);
    }

    private int GetNextId()
        => _pairingRepository.GetLastId();
}
