using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.CageService;

public class CageService(
        IUnitOfWork unitOfWork,
        int sacrificableAgeInDays) 
    : ICageService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly int _sacrificableAgeInDays = sacrificableAgeInDays;

    public async Task<Result<Cage>> CreateCage(string name)
    {
        var createdCage = await _unitOfWork.CageRepository.AddAsync(new Cage(name));
        return Result.Success(createdCage);
    }

    public async Task<Result<Cage>> GetCageById(int cageId)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<Cage>(CageErrors.NotFound);

        cage.UpdateSacrificableStatus(_sacrificableAgeInDays);

        return Result.Success(cage);
    }

    public async Task<Result<Cage>> UpdateOffspringType(int cageId, OffspringType offspringType)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<Cage>(CageErrors.NotFound);

        cage.OffspringType = offspringType;
        await _unitOfWork.CageRepository.UpdateAsync(cage);

        return Result.Success(cage);
    }

    public async Task<Result<Cage>> MoveBreedingRabbitToCage(int breedingRabbitId, int destinationCageId)
    {
        var breedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
        if (breedingRabbit == null)
            return Result.Failure<Cage>(BreedingRabbitErrors.NotFound);

        var destinationCage = await _unitOfWork.CageRepository.GetByIdAsync(destinationCageId);
        if (destinationCage == null)
            return Result.Failure<Cage>(CageErrors.NotFound);

        if (breedingRabbit.CageId != null)
        {
            var sourceCage = await _unitOfWork.CageRepository.GetByIdAsync(breedingRabbit.CageId.Value);
            if (sourceCage == null)
                return Result.Failure<Cage>(CageErrors.NotFound);

            var removalResult = sourceCage.RemoveBreedingRabbit();
            if (removalResult.IsFailure)
                return Result.Failure<Cage>(removalResult.Error);

            await _unitOfWork.CageRepository.UpdateAsync(sourceCage);
            breedingRabbit = removalResult.Value;
        }

        var assignmentResult = destinationCage.AssignBreedingRabbit(breedingRabbit);
        if (assignmentResult.IsFailure)
            return Result.Failure<Cage>(assignmentResult.Error);

        await _unitOfWork.CageRepository.UpdateAsync(destinationCage);
        await _unitOfWork.BreedingRabbitRepository.UpdateAsync(breedingRabbit);

        return Result.Success(destinationCage);
    }

    public async Task<Result<PaginatedResult<Cage>>> GetPaginatedCages(PaginatedRequest<CageFilterDto> request)
    {
        var cages = await _unitOfWork.CageRepository.GetPaginatedAsync(request);

        foreach (var cage in cages.Items)
            cage.UpdateSacrificableStatus(_sacrificableAgeInDays);

        return Result.Success(cages);
    }

    public async Task<Result<Cage>> SacrificeOffspring(int cageId, int count)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<Cage>(CageErrors.NotFound);

        cage.UpdateSacrificableStatus(_sacrificableAgeInDays);

        var sacrificeResult = cage.SacrificeOffspring(count);
        if (sacrificeResult.IsFailure)
            return Result.Failure<Cage>(sacrificeResult.Error);

        await _unitOfWork.CageRepository.UpdateAsync(cage);
        return Result.Success(cage);
    }
}
