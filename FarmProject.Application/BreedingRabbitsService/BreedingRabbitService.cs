using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.BreedingRabbitsService;

public class BreedingRabbitService(IUnitOfWork unitOfWork) : IBreedingRabbitService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<BreedingRabbit>> AddBreedingRabbitToFarm(string name, int cageId)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
            if (cage == null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<BreedingRabbit>(CageErrors.NotFound);
            }

            var breedingRabbit = new BreedingRabbit(name);
            var createdRabbit = await _unitOfWork.BreedingRabbitRepository.AddAsync(breedingRabbit);

            var assignmentResult = cage.AssignBreedingRabbit(createdRabbit);
            if (assignmentResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<BreedingRabbit>(assignmentResult.Error);
            }
            await _unitOfWork.CageRepository.UpdateAsync(cage);

            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(createdRabbit);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Failure<BreedingRabbit>(
                new Error("BreedingRabbit.CreationFailed", ex.Message));
        }
    }

    public async Task<Result<List<BreedingRabbit>>> GetAllBreedingRabbits()
    {
        var breedingRabbits = await _unitOfWork.BreedingRabbitRepository.GetAllAsync();
        return Result.Success(breedingRabbits);
    }

    public async Task<Result<BreedingRabbit>> GetBreedingRabbitById(int breedingRabbitId)
    {
        var requestBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
        if (requestBreedingRabbit == null)
            return Result.Failure<BreedingRabbit>(BreedingRabbitErrors.NotFound);

        return Result.Success(requestBreedingRabbit);
    }

    public async Task<Result<BreedingRabbit>> UpdateBreedingStatus(int breedingRabbitId, BreedingStatus breedingStatus)
    {
        var requestBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
        if (requestBreedingRabbit == null)
            return Result.Failure<BreedingRabbit>(BreedingRabbitErrors.NotFound);

        requestBreedingRabbit.SetBreedingStatus(breedingStatus);

        var updatedBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.UpdateAsync(requestBreedingRabbit);

        return Result.Success(updatedBreedingRabbit);
    }
}
