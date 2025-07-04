using FarmProject.Application.Events;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;


namespace FarmProject.Application.BirthService;

public class BirthService(
        IUnitOfWork unitOfWork,
        DomainEventDispatcher domainEventDispatcher)
    : IBirthService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly DomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;

    public async Task<Result<BreedingRabbit>> RecordBirth(int breedingRabbitId, int offspringCount)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var breedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
            if (breedingRabbit == null)
                return Result.Failure<BreedingRabbit>(BreedingRabbitErrors.NotFound);

            var cage = await _unitOfWork.CageRepository.GetByIdAsync(breedingRabbit.CageId ?? 0);
            if (cage == null)
                return Result.Failure<BreedingRabbit>(CageErrors.NotFound);

            var birthResult = breedingRabbit.RecordBirth(offspringCount, DateTime.Now);
            if (birthResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<BreedingRabbit>(birthResult.Error);
            }

            var offspringResult = cage.AddOffspring(offspringCount);
            if (offspringResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<BreedingRabbit>(offspringResult.Error);
            }

            cage.OffspringType = OffspringType.Mixed;

            await _unitOfWork.BreedingRabbitRepository.UpdateAsync(breedingRabbit);
            await _unitOfWork.CageRepository.UpdateAsync(cage);

            await _domainEventDispatcher.DispatchEventsAsync(breedingRabbit.DomainEvents);

            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(breedingRabbit);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<Result> WeanOffspring(int oldCageId, int newCageId)
    {
        var oldCage = await _unitOfWork.CageRepository.GetByIdAsync(oldCageId);
        if (oldCage == null)
            return Result.Failure(CageErrors.NotFound);

        if (oldCage.BreedingRabbit == null || oldCage.BreedingRabbit.BreedingStatus != BreedingStatus.Nursing)
            return Result.Failure(CageErrors.NoBreedingRabbit);

            var newCage = await _unitOfWork.CageRepository.GetByIdAsync(newCageId);
        if (newCage == null)
            return Result.Failure(CageErrors.NotFound);

        if (newCage.BreedingRabbit != null || newCage.OffspringType != OffspringType.None)
            return Result.Failure(CageErrors.Occupied);

        var offspringAmount = oldCage.OffspringCount;

        oldCage.RemoveOffspring(offspringAmount);
        oldCage.OffspringType = OffspringType.None;
        newCage.AddOffspring(offspringAmount);
        newCage.OffspringType = OffspringType.Mixed;
        oldCage.BreedingRabbit.BreedingStatus = BreedingStatus.Recovering;

        await _unitOfWork.BreedingRabbitRepository.UpdateAsync(oldCage.BreedingRabbit);
        await _unitOfWork.CageRepository.UpdateAsync(oldCage);
        await _unitOfWork.CageRepository.UpdateAsync(newCage);

        return Result.Success();
    }

    public async Task<Result> SeparateOffspring(int currentCageId, int? otherCageId, int? femaleOffspringCount)
    {
        var currentOffspringCage = await _unitOfWork.CageRepository.GetByIdAsync(currentCageId);
        if (currentOffspringCage == null)
            return Result.Failure(CageErrors.NotFound);

        var currentOffspringAmount = currentOffspringCage.OffspringCount;
        if (femaleOffspringCount.HasValue && femaleOffspringCount > currentOffspringAmount)
            return Result.Failure(CageErrors.InvalidSeparation);

        var otherOffspringCage = await _unitOfWork.CageRepository.GetByIdAsync(otherCageId ?? 0);

        if (femaleOffspringCount.HasValue && femaleOffspringCount == currentOffspringAmount)
        {
            currentOffspringCage.OffspringType = OffspringType.Female;
            await _unitOfWork.CageRepository.UpdateAsync(currentOffspringCage);
            return Result.Success();
        }
        else if (!femaleOffspringCount.HasValue || femaleOffspringCount <= 0)
        {
            currentOffspringCage.OffspringType = OffspringType.Male;
            await _unitOfWork.CageRepository.UpdateAsync(currentOffspringCage);
            return Result.Success();
        }

        if (otherOffspringCage == null)
            return Result.Failure(CageErrors.InvalidSeparation);

        if (otherOffspringCage.BreedingRabbit != null || otherOffspringCage.OffspringType != OffspringType.None)
            return Result.Failure(CageErrors.Occupied);

        currentOffspringCage.RemoveOffspring(femaleOffspringCount.Value);
        currentOffspringCage.OffspringType = OffspringType.Male;
        otherOffspringCage.AddOffspring(femaleOffspringCount.Value);
        otherOffspringCage.OffspringType = OffspringType.Female;

        await _unitOfWork.CageRepository.UpdateAsync(currentOffspringCage);
        await _unitOfWork.CageRepository.UpdateAsync(otherOffspringCage);

        return Result.Success();
    }
}
