using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public class PairingService(IUnitOfWork unitOfWork, IRabbitService rabbitService) : IPairingService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRabbitService _rabbitService = rabbitService;

    public async Task<Result<Pair>> CreatePair(int firstAnimalId, int secondAnimalId)
    {
        // Get the rabbits
        var firstAnimalResult = await _rabbitService.GetRabbitById(firstAnimalId);
        var secondAnimalResult = await _rabbitService.GetRabbitById(secondAnimalId);

        if (firstAnimalResult.IsFailure)
            return Result.Failure<Pair>(firstAnimalResult.Error);

        if (secondAnimalResult.IsFailure)
            return Result.Failure<Pair>(secondAnimalResult.Error);

        var firstAnimal = firstAnimalResult.Value;
        var secondAnimal = secondAnimalResult.Value;

        // Breed the rabbits
        var bredPairResult = firstAnimal.Breed(secondAnimal, DateTime.Now);

        if (bredPairResult.IsFailure)
            return Result.Failure<Pair>(bredPairResult.Error);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Update the rabbits
            await _unitOfWork.RabbitRepository.UpdateAsync(firstAnimal);
            await _unitOfWork.RabbitRepository.UpdateAsync(secondAnimal);

            // Create the pair
            var createdPair = await _unitOfWork.PairingRepository.AddAsync(bredPairResult.Value);

            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(createdPair);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
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
            // Create nest prep FarmEvent & Record pairing outcome
            if (pairingStatus == PairingStatus.Successful)
            {
                // Update PairingStatus of the Pair
                var recordPairResult = requestPair.RecordSuccessfulImpregnation(DateTime.Now);

                if (recordPairResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure<Pair>(recordPairResult.Error);
                }

                // Create the FarmEvent instance
                var createNestPrepEventResult = requestPair.CreateNestPrepEvent();

                if (createNestPrepEventResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure<Pair>(createNestPrepEventResult.Error);
                }

                // Create FarmEvent record in the repo
                await _unitOfWork.FarmEventRepository.AddAsync(createNestPrepEventResult.Value);
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

            // Update rabbits references
            await _unitOfWork.RabbitRepository.UpdateAsync(requestPair.MaleRabbit!);
            await _unitOfWork.RabbitRepository.UpdateAsync(requestPair.FemaleRabbit!);

            // Update the pair
            var updatedPair = await _unitOfWork.PairingRepository.UpdateAsync(requestPair);
            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(updatedPair);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
