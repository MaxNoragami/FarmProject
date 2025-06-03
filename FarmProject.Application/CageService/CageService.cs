using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.CageService;

public class CageService(IUnitOfWork unitOfWork) : ICageService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Cage>> CreateCage(string name)
    {
        var createdCage = await _unitOfWork.CageRepository.AddAsync(new Cage(name));
        return Result.Success(createdCage);
    }

    public async Task<Result<List<Cage>>> GetAllCages()
    {
        var cages = await _unitOfWork.CageRepository.GetAllAsync();
        return Result.Success(cages);
    }

    public async Task<Result<Cage>> GetCageById(int cageId)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<Cage>(CageErrors.NotFound);
        return Result.Success(cage);
    }

    public async Task<Result<List<Cage>>> GetUnoccupiedCages(Gender gender)
    {
        var specification = new CageSpecificationByUnoccupied(gender);
        var unoccupiedCages = await _unitOfWork.CageRepository.FindAsync(specification);
        return Result.Success(unoccupiedCages);
    }

    public async Task<Result<Cage>> AddOffspringsToCage(int cageId, int count)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<Cage>(CageErrors.NotFound);

        var additionResult = cage.AddOffspring(count);

        if (additionResult.IsFailure)
            return Result.Failure<Cage>(additionResult.Error);

        return Result.Success(cage);
    }

    public async Task<Result<Cage>> AssignBreedingRabbitToCage(int cageId, int breedingRabbitId)
    {
        var breedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
        if (breedingRabbit == null)
            return Result.Failure<Cage>(BreedingRabbitErrors.NotFound);

        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<Cage>(CageErrors.NotFound);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var assignmentResult = cage.AssignBreedingRabbit(breedingRabbit);
            if (assignmentResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<Cage>(assignmentResult.Error);
            }

            await _unitOfWork.BreedingRabbitRepository.UpdateAsync(breedingRabbit);

            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(cage);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Cage>(new Error("Cage.Failed", ex.Message));
        }
    }

    public async Task<Result<Cage>> RemoveBreedingRabbitFromCage(int cageId, Gender gender)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<Cage>(CageErrors.NotFound);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var removalResult = cage.RemoveBreedingRabbit(gender);
            if (removalResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<Cage>(removalResult.Error);
            }

            await _unitOfWork.BreedingRabbitRepository.UpdateAsync(removalResult.Value);

            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(cage);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Cage>(new Error("Cage.Failed", ex.Message));
        }
    }

    public async Task<Result<Cage>> RemoveOffspringsFromCage(int cageId, int count)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<Cage>(CageErrors.NotFound);

        var subtractionResult = cage.RemoveOffspring(count);
        
        if (subtractionResult.IsFailure)
            return Result.Failure<Cage>(subtractionResult.Error);

        return Result.Success(cage);
    }

    public async Task<Result<Cage>> UpdateOffspringType(int cageId, OffspringType offspringType)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<Cage>(CageErrors.NotFound);

        cage.SetOffspringType(offspringType);
        await _unitOfWork.CageRepository.UpdateAsync(cage);

        return Result.Success(cage);
    }
}
