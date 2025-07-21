using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Events;
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

    public async Task<Result<Pair>> CreatePair(int breedingRabbitId, int maleRabbitId)
    {
        var breedingRabbitResult = await _breedingRabbitService.GetBreedingRabbitById(breedingRabbitId);

        if (breedingRabbitResult.IsFailure)
            return Result.Failure<Pair>(breedingRabbitResult.Error);

        var breedingRabbit = breedingRabbitResult.Value;

        var breedResult = breedingRabbit.Breed(maleRabbitId, DateTime.Now);

        if (breedResult.IsFailure)
            return Result.Failure<Pair>(breedResult.Error);

        await _unitOfWork.BreedingRabbitRepository.UpdateAsync(breedingRabbit);

        await _domainEventDispatcher.DispatchEventsAsync(breedingRabbit.DomainEvents);

        var createdPair = await _unitOfWork.PairingRepository
            .GetMostRecentPairByBreedingRabbitIdsAsync(breedingRabbit.Id, maleRabbitId);

        if (createdPair == null)
            return Result.Failure<Pair>(PairErrors.CreationFailed);

        return Result.Success(createdPair);
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
        var requestPair = await _unitOfWork.PairingRepository.GetByIdAsync(pairId);

        if (requestPair == null)
            return Result.Failure<Pair>(PairErrors.NotFound);

        if (pairingStatus == PairingStatus.Successful)
        {
            var recordPairResult = requestPair.RecordSuccessfulImpregnation(DateTime.Now);

            if (recordPairResult.IsFailure)
                return Result.Failure<Pair>(recordPairResult.Error);

            var createNestPrepTaskResult = requestPair.CreateNestPrepTask();

            if (createNestPrepTaskResult.IsFailure)
                return Result.Failure<Pair>(createNestPrepTaskResult.Error);
        }
        else if (pairingStatus == PairingStatus.Failed)
        {
            var recordPairResult = requestPair.RecordFailedImpregnation(DateTime.Now);

            if (recordPairResult.IsFailure)
                return Result.Failure<Pair>(recordPairResult.Error);
        }
        else
            return Result.Failure<Pair>(PairErrors.InvalidOutcome);

        await _unitOfWork.BreedingRabbitRepository.UpdateAsync(requestPair.FemaleRabbit!);

        await _domainEventDispatcher.DispatchEventsAsync(requestPair.DomainEvents);

        var updatedPair = await _unitOfWork.PairingRepository.UpdateAsync(requestPair);
        return Result.Success(updatedPair);
    }
}
