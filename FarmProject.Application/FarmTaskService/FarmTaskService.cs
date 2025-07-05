using FarmProject.Application.BirthService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.FarmTaskService;

public class FarmTaskService(
        IUnitOfWork unitOfWork,
        IBirthService birthService
    ) : IFarmTaskService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBirthService _birthService = birthService;

    public async Task<Result<FarmTask>> CreateFarmTask(FarmTask farmTask)
    {
        if (farmTask == null)
            return Result.Failure<FarmTask>(FarmTaskErrors.NullValue);

        var createdTask = await _unitOfWork.FarmTaskRepository.AddAsync(farmTask);
        return Result.Success(createdTask);
    }

    public async Task<Result<FarmTask>> GetFarmTaskById(int taskId)
    {
        var requestTask = await _unitOfWork.FarmTaskRepository.GetByIdAsync(taskId);
        if (requestTask == null)
            return Result.Failure<FarmTask>(FarmTaskErrors.NotFound);
        return Result.Success(requestTask);
    }

    public async Task<Result<PaginatedResult<FarmTask>>> GetPaginatedFarmTasks(PaginatedRequest<FarmTaskFilterDto> request)
    {
        var farmTasks = await _unitOfWork.FarmTaskRepository.GetPaginatedAsync(request);

        return Result.Success(farmTasks);
    }

    public async Task<Result<FarmTask>> MarkFarmTaskAsCompleted(int taskId, CompleteTaskData? completeTaskData = null)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var requestTask = await _unitOfWork.FarmTaskRepository.GetByIdAsync(taskId);

            if (requestTask == null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<FarmTask>(FarmTaskErrors.NotFound);
            }
                

            if (requestTask.IsCompleted)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<FarmTask>(FarmTaskErrors.AlreadyCompleted);
            }

            var taskActionResult = requestTask.FarmTaskType switch
            {
                FarmTaskType.Weaning => await HandleWeaningCompletion(requestTask, completeTaskData),
                FarmTaskType.OffspringSeparation => await HandleOffspringSeparationCompletion(requestTask, completeTaskData),
                _ => Result.Success()
            };

            if (taskActionResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure<FarmTask>(taskActionResult.Error);
            }

            var taskMarkCompletedResult = requestTask.MarkAsCompleted();
            if (taskMarkCompletedResult.IsFailure)
                return Result.Failure<FarmTask>(taskMarkCompletedResult.Error);

            var updateTask = await _unitOfWork.FarmTaskRepository.UpdateAsync(requestTask);

            await _unitOfWork.CommitTransactionAsync();
            return Result.Success(updateTask);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private async Task<Result> HandleWeaningCompletion(FarmTask farmTask, CompleteTaskData? completeTaskData)
    {
        if (farmTask.CageId == null || completeTaskData?.NewCageId == null)
            return Result.Failure(FarmTaskErrors.MissingParameter);

        return await _birthService.WeanOffspring
            (farmTask.CageId.Value, 
            completeTaskData.NewCageId.Value
        );
    }

    private async Task<Result> HandleOffspringSeparationCompletion(FarmTask farmTask, CompleteTaskData? completeTaskData)
    {
        if (farmTask.CageId == null || completeTaskData == null)
            return Result.Failure(FarmTaskErrors.MissingParameter);

        return await _birthService.SeparateOffspring(
            farmTask.CageId.Value,
            completeTaskData.OtherCageId,
            completeTaskData.FemaleOffspringCount
        );
    }
}
