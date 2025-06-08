using FarmProject.Application.Common;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.BreedingRabbitsService;

public class BreedingRabbitService(
        IUnitOfWork unitOfWork,
        LoggingHelper loggingHelper
    ) : IBreedingRabbitService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<BreedingRabbit>> AddBreedingRabbitToFarm(string name, int cageId)
    {
        return await _loggingHelper.LogOperation(
            $"AddBreedingRabbitToFarm({name},{cageId})",
            async () =>
        {
            try
            {
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
        });
    }

    public async Task<Result<List<BreedingRabbit>>> GetAllBreedingRabbits()
    {
        return await _loggingHelper.LogOperation(
            $"GetAllBreedingRabbits()",
            async () =>
        {
            var breedingRabbits = await _unitOfWork.BreedingRabbitRepository.GetAllAsync();
            return Result.Success(breedingRabbits);
        });
    }

    public async Task<Result<List<BreedingRabbit>>> GetAllAvailableBreedingRabbits()
    {
        return await _loggingHelper.LogOperation(
            $"GetAllAvailableBreedingRabbits()",
            async () =>
        {
            var specification = new BreedingRabbitSpecificationByAvailable();
            var availableBreedingRabbits = await _unitOfWork
                .BreedingRabbitRepository.FindAsync(specification);
            return Result.Success(availableBreedingRabbits);
        });
    }

    public async Task<Result<BreedingRabbit>> GetBreedingRabbitById(int breedingRabbitId)
    {
        return await _loggingHelper.LogOperation(
            $"GetBreedingRabbitById({breedingRabbitId})",
            async () =>
        {
            var requestBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
            if (requestBreedingRabbit == null)
                return Result.Failure<BreedingRabbit>(BreedingRabbitErrors.NotFound);

            return Result.Success(requestBreedingRabbit);
        });
    }

    public async Task<Result<BreedingRabbit>> UpdateBreedingStatus(int breedingRabbitId, BreedingStatus breedingStatus)
    {
        return await _loggingHelper.LogOperation(
            $"UpdateBreedingStatus({breedingRabbitId},{breedingStatus})",
            async () =>
        {
            var requestBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
            if (requestBreedingRabbit == null)
                return Result.Failure<BreedingRabbit>(BreedingRabbitErrors.NotFound);

            requestBreedingRabbit.BreedingStatus = breedingStatus;

            var updatedBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.UpdateAsync(requestBreedingRabbit);

            return Result.Success(updatedBreedingRabbit);
        });
    }
}
