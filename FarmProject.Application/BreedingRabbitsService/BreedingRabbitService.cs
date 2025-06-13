using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.BreedingRabbitsService;

public class BreedingRabbitService(IUnitOfWork unitOfWork) : IBreedingRabbitService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<BreedingRabbit>> AddBreedingRabbitToFarm(string name, int cageId)
    {
        try {     
            await _unitOfWork.BeginTransactionAsync();
            var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
            if (cage == null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<BreedingRabbit>(CageErrors.NotFound);
            }

            var breedingRabbit = new BreedingRabbit(name);

            var assignmentResult = cage.AssignBreedingRabbit(breedingRabbit);
            if (assignmentResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<BreedingRabbit>(assignmentResult.Error);
            }
            var createdRabbit = await _unitOfWork.BreedingRabbitRepository.AddAsync(breedingRabbit);
            await _unitOfWork.CageRepository.UpdateAsync(cage);

            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(createdRabbit);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<Result<List<BreedingRabbit>>> GetAllBreedingRabbits()
    {
        var breedingRabbits = await _unitOfWork.BreedingRabbitRepository.GetAllAsync();
        return Result.Success(breedingRabbits);
    }

    public async Task<Result<List<BreedingRabbit>>> GetAllAvailableBreedingRabbits()
    {
        var specification = new BreedingRabbitSpecificationByAvailable();
        var availableBreedingRabbits = await _unitOfWork
            .BreedingRabbitRepository.FindAsync(specification);
        return Result.Success(availableBreedingRabbits);
    }

    public async Task<Result<BreedingRabbit>> GetBreedingRabbitById(int breedingRabbitId)
    {
        var requestBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
        if (requestBreedingRabbit == null)
            return Result.Failure<BreedingRabbit>(BreedingRabbitErrors.NotFound);

        return Result.Success(requestBreedingRabbit);
    }

    public async Task<Result<BreedingRabbit>> UpdateBreedingStatus(
        int breedingRabbitId, BreedingStatus breedingStatus)
    {
        var requestBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
        if (requestBreedingRabbit == null)
            return Result.Failure<BreedingRabbit>(BreedingRabbitErrors.NotFound);

        requestBreedingRabbit.BreedingStatus = breedingStatus;

        var updatedBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.UpdateAsync(requestBreedingRabbit);

        return Result.Success(updatedBreedingRabbit);
    }

    public async Task<Result<PaginatedResult<BreedingRabbit>>> GetPaginatedBreedingRabbits(
        PaginatedRequest<BreedingRabbitFilterDto> request)
    {
        var breedingRabbits = await _unitOfWork.BreedingRabbitRepository.GetPaginatedAsync(request);

        return Result.Success(breedingRabbits);
    }
}
