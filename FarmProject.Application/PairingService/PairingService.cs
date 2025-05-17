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
        // Get the rabbits
        var firstAnimalResult = await _animalService.GetRabbitById(firstAnimalId);
        var secondAnimalResult = await _animalService.GetRabbitById(secondAnimalId);

        if (firstAnimalResult.IsFailure)
            return Result.Failure<Pair>(firstAnimalResult.Error);

        if (secondAnimalResult.IsFailure)
            return Result.Failure<Pair>(secondAnimalResult.Error);

        var firstAnimal = firstAnimalResult.Value;
        var secondAnimal = secondAnimalResult.Value;

        // Breed the rabbits
        var bredPairResult = firstAnimal.Breed(secondAnimal, GetNextId(), DateTime.Now);

        if (bredPairResult.IsFailure)
            return Result.Failure<Pair>(bredPairResult.Error);

        // Update the rabbits
        var firstUpdateResult = await _animalService.UpdateRabbit(firstAnimal);
        if (firstUpdateResult.IsFailure)
            return Result.Failure<Pair>(firstUpdateResult.Error);

        var secondUpdateResult = await _animalService.UpdateRabbit(secondAnimal);
        if (secondUpdateResult.IsFailure)
            return Result.Failure<Pair>(secondUpdateResult.Error);

        // Create the pair
        var createdPair = _pairingRepository.Create(bredPairResult.Value);

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
        // Get the Pair
        var requestPair = _pairingRepository.GetById(pairId);

        if (requestPair == null)
            return Result.Failure<Pair>(PairErrors.NotFound);

        // Get the rabbits
        var femaleRabbitResult = await _animalService.GetRabbitById(requestPair.FemaleId);
        var maleRabbitResult = await _animalService.GetRabbitById(requestPair.MaleId);

        if (femaleRabbitResult.IsFailure)
            return Result.Failure<Pair>(femaleRabbitResult.Error);

        if (maleRabbitResult.IsFailure)
            return Result.Failure<Pair>(maleRabbitResult.Error);

        var femaleRabbit = femaleRabbitResult.Value;
        var maleRabbit = maleRabbitResult.Value;

        // Update PairingStatus of the Pair
        var completePairResult = requestPair.CompletePairing(pairingStatus, maleRabbit, femaleRabbit, DateTime.Now);

        if (completePairResult.IsFailure)
            return Result.Failure<Pair>(completePairResult.Error);

        // Update the rabbits
        var femaleUpdateResult = await _animalService.UpdateRabbit(femaleRabbit);
        if (femaleUpdateResult.IsFailure)
            return Result.Failure<Pair>(femaleUpdateResult.Error);

        var maleUpdateResult = await _animalService.UpdateRabbit(maleRabbit);
        if (maleUpdateResult.IsFailure)
            return Result.Failure<Pair>(maleUpdateResult.Error);

        // Update the pair
        var updatedPair = _pairingRepository.Update(requestPair);
        return Result.Success(updatedPair);
    }

    private int GetNextId()
        => _pairingRepository.GetLastId();
}
