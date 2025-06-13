using FarmProject.Application.Events;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;

namespace FarmProject.Application.PairingService;

public class PairingService(IUnitOfWork unitOfWork, 
        IBreedingRabbitService breedingRabbitService,
        DomainEventDispatcher domainEventDispatcher)
    : IPairingService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;
    private readonly DomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;

    public async Task<Result<Pair>> CreatePair(int breedingRabbitId, int maleRabbitId)
    {
        // Get the breeding rabbits
        var breedingRabbitResult = await _breedingRabbitService.GetBreedingRabbitById(breedingRabbitId);

        if (breedingRabbitResult.IsFailure)
            return Result.Failure<Pair>(breedingRabbitResult.Error);

        var breedingRabbit = breedingRabbitResult.Value;

        // Breed the breeding rabbits
        var breedResult = breedingRabbit.Breed(maleRabbitId, DateTime.Now);

        if (breedResult.IsFailure)
            return Result.Failure<Pair>(breedResult.Error);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Update the breeding rabbit
            await _unitOfWork.BreedingRabbitRepository.UpdateAsync(breedingRabbit);

            await _domainEventDispatcher.DispatchEventsAsync(breedingRabbit.DomainEvents);

            var createdPair = await _unitOfWork.PairingRepository
                .GetMostRecentPairByBreedingRabbitIdsAsync(breedingRabbit.Id, maleRabbitId);

            if (createdPair == null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<Pair>(PairErrors.CreationFailed);
            }

            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(createdPair);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<Result<PaginatedResult<Pair>>> GetPaginatedPairs(PaginatedRequest<PairFilterDto> request)
    {
        var pairs = await _unitOfWork.PairingRepository.GetPaginatedAsync(request);

        return Result.Success(pairs);
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
        try
        {
            await _unitOfWork.BeginTransactionAsync();

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
            await _unitOfWork.BreedingRabbitRepository.UpdateAsync(requestPair.FemaleRabbit!);

            await _domainEventDispatcher.DispatchEventsAsync(requestPair.DomainEvents);

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
