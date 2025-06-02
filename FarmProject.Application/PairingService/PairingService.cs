using FarmProject.Application.Events;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public class PairingService(IUnitOfWork unitOfWork, 
        IBreedingRabbitService breedingRabbitService,
        DomainEventDispatcher domainEventDispatcher) 
    : IPairingService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;
    private readonly DomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;

    public async Task<Result<Pair>> CreatePair(int firstAnimalId, int secondAnimalId)
    {
        // Get the breeding rabbits
        var firstAnimalResult = await _breedingRabbitService.GetBreedingRabbitById(firstAnimalId);
        var secondAnimalResult = await _breedingRabbitService.GetBreedingRabbitById(secondAnimalId);

        if (firstAnimalResult.IsFailure)
            return Result.Failure<Pair>(firstAnimalResult.Error);

        if (secondAnimalResult.IsFailure)
            return Result.Failure<Pair>(secondAnimalResult.Error);

        var firstAnimal = firstAnimalResult.Value;
        var secondAnimal = secondAnimalResult.Value;

        // Breed the breeding rabbits
        var breedResult = firstAnimal.Breed(secondAnimal, DateTime.Now);

        if (breedResult.IsFailure)
            return Result.Failure<Pair>(breedResult.Error);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Update the breeding rabbits
            await _unitOfWork.BreedingRabbitRepository.UpdateAsync(firstAnimal);
            await _unitOfWork.BreedingRabbitRepository.UpdateAsync(secondAnimal);

            await _domainEventDispatcher.DispatchEventsAsync(firstAnimal.DomainEvents);

            var createdPair = await _unitOfWork.PairingRepository
                .GetMostRecentPairByBreedingRabbitIdsAsync(firstAnimal.Id, secondAnimal.Id);

            if (createdPair == null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<Pair>(PairErrors.CreationFailed);
            }

            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(createdPair);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Pair>(new Error("Pair.Failed", ex.Message));
        }
    }

    public async Task<Result<List<Pair>>> GetAllPairs()
    {
        var requestPairs = await _unitOfWork.PairingRepository.GetAllAsync();
        return Result.Success(requestPairs);
    }

    public async Task<Result<Pair>> GetPairById(int pairId)
    {
        var requestPair = await _unitOfWork.PairingRepository.GetByIdAsync(pairId);

        if (requestPair == null)
            return Result.Failure<Pair>(PairErrors.NotFound);

        return Result.Success(requestPair);
    }

    public async Task<Result<Pair>> UpdatePairingStatus(int pairId, PairingStatus pairingStatus)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Get the Pair
            var requestPair = await _unitOfWork.PairingRepository.GetByIdAsync(pairId);

            if (requestPair == null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<Pair>(PairErrors.NotFound);
            }
            // Create nest prep FarmTask & Record pairing outcome
            if (pairingStatus == PairingStatus.Successful)
            {
                // Update PairingStatus of the Pair
                var recordPairResult = requestPair.RecordSuccessfulImpregnation(DateTime.Now);

                if (recordPairResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure<Pair>(recordPairResult.Error);
                }

                // Create the FarmTask instance
                var createNestPrepTaskResult = requestPair.CreateNestPrepTask();

                if (createNestPrepTaskResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure<Pair>(createNestPrepTaskResult.Error);
                }
            }
            else if (pairingStatus == PairingStatus.Failed)
            {
                // Update PairingStatus of the Pair
                var recordPairResult = requestPair.RecordFailedImpregnation(DateTime.Now);

                if (recordPairResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure<Pair>(recordPairResult.Error);
                }
            }
            else
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<Pair>(PairErrors.InvalidOutcome);
            }

            // Update breeding rabbits references
            await _unitOfWork.BreedingRabbitRepository.UpdateAsync(requestPair.MaleBreedingRabbit!);
            await _unitOfWork.BreedingRabbitRepository.UpdateAsync(requestPair.FemaleBreedingRabbit!);

            await _domainEventDispatcher.DispatchEventsAsync(requestPair.DomainEvents);

            // Update the pair
            var updatedPair = await _unitOfWork.PairingRepository.UpdateAsync(requestPair);
            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(updatedPair);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Pair>(new Error("Pair.UpdateFailed", ex.Message));
        }
    }
}
