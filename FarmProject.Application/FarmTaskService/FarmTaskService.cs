using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.FarmTaskService;

public class FarmTaskService(IUnitOfWork unitOfWork) : IFarmTaskService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

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

    public async Task<Result<FarmTask>> MarkFarmTaskAsCompleted(int taskId)
    {
        var requestTask = await _unitOfWork.FarmTaskRepository.GetByIdAsync(taskId);

        if (requestTask is null)
            return Result.Failure<FarmTask>(FarmTaskErrors.NotFound);

        var taskMarkCompletedResult = requestTask.MarkAsCompleted();
        if (taskMarkCompletedResult.IsFailure)
            return Result.Failure<FarmTask>(taskMarkCompletedResult.Error);

        var updateTask = await _unitOfWork.FarmTaskRepository.UpdateAsync(requestTask);

        return Result.Success(updateTask);
    }  
}
