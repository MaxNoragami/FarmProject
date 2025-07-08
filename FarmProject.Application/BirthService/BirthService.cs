using FarmProject.Application.Events;
using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FarmProject.Domain.Services;


namespace FarmProject.Application.BirthService;

public class BirthService(
        IUnitOfWork unitOfWork,
        DomainEventDispatcher domainEventDispatcher,
        BirthDomainService birthDomainService)
    : IBirthService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly DomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;
    private readonly BirthDomainService _birthDomainService = birthDomainService;

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

            var result = _birthDomainService.RecordBirth(breedingRabbit, cage, offspringCount, DateTime.Now);
            if (result.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<BreedingRabbit>(result.Error);
            }

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

        var newCage = await _unitOfWork.CageRepository.GetByIdAsync(newCageId);
        if (newCage == null)
            return Result.Failure(CageErrors.NotFound);

        var result = _birthDomainService.WeanOffspring(oldCage, newCage, DateTime.Now);
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _unitOfWork.BreedingRabbitRepository.UpdateAsync(oldCage.BreedingRabbit!);
        await _unitOfWork.CageRepository.UpdateAsync(oldCage);
        await _unitOfWork.CageRepository.UpdateAsync(newCage);

        await _domainEventDispatcher.DispatchEventsAsync([result.Value]);

        return Result.Success();
    }

    public async Task<Result> SeparateOffspring(int currentCageId, int? otherCageId, int? femaleOffspringCount)
    {
        var currentOffspringCage = await _unitOfWork.CageRepository.GetByIdAsync(currentCageId);
        if (currentOffspringCage == null)
            return Result.Failure(CageErrors.NotFound);

        Cage? otherOffspringCage = null;
        if (otherCageId.HasValue)
        {
            otherOffspringCage = await _unitOfWork.CageRepository.GetByIdAsync(otherCageId.Value);
            if (otherOffspringCage == null)
                return Result.Failure(CageErrors.NotFound);
        }

        var result = _birthDomainService.SeparateOffspring(
            currentOffspringCage,
            otherOffspringCage,
            femaleOffspringCount);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _unitOfWork.CageRepository.UpdateAsync(currentOffspringCage);
        if (otherOffspringCage != null)
            await _unitOfWork.CageRepository.UpdateAsync(otherOffspringCage);

        return Result.Success();
    }
}
