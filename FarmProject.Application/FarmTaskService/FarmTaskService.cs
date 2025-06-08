using FarmProject.Application.Common;
using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.FarmTaskService;

public class FarmTaskService(
        IUnitOfWork unitOfWork,
        LoggingHelper loggingHelper
    ) : IFarmTaskService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<FarmTask>> CreateFarmTask(FarmTask farmTask)
    {
        return await _loggingHelper.LogOperation(
            $"CreateFarmTask({farmTask})",
            async () =>
        {
            if (farmTask == null)
                return Result.Failure<FarmTask>(FarmTaskErrors.NullValue);

            var createdTask = await _unitOfWork.FarmTaskRepository.AddAsync(farmTask);
            return Result.Success(createdTask);
        });
    }

    public async Task<Result<List<FarmTask>>> GetAllFarmTasks()
    {
        return await _loggingHelper.LogOperation(
            $"GetAllFarmTasks()",
            async () =>
        {
            var allFarmTasks = await _unitOfWork.FarmTaskRepository.GetAllAsync();
            return Result.Success(allFarmTasks);
        });
    }

    public async Task<Result<List<FarmTask>>> GetAllFarmTasksByDate(DateTime date)
    {
        return await _loggingHelper.LogOperation(
            $"GetAllFarmTasksByDate({date})",
            async () =>
        {
            var specification = new FarmTaskSpecificationsByDate(date);
            var tasksOnDate = await _unitOfWork.FarmTaskRepository.FindAsync(specification);
            return Result.Success(tasksOnDate);
        });
    }

    public async Task<Result<List<FarmTask>>> GetAllPendingFarmTasks()
    {
        return await _loggingHelper.LogOperation(
            $"GetAllPendingFarmTasks()",
            async () =>
        {
            var specification = new FarmTaskSpecificationPending();
            var tasksPending = await _unitOfWork.FarmTaskRepository.FindAsync(specification);
            return Result.Success(tasksPending);
        });
    }

    public async Task<Result<FarmTask>> GetFarmTaskById(int taskId)
    {
        return await _loggingHelper.LogOperation(
            $"GetFarmTaskById({taskId})",
            async () =>
        {
            var requestTask = await _unitOfWork.FarmTaskRepository.GetByIdAsync(taskId);
            if (requestTask == null)
                return Result.Failure<FarmTask>(FarmTaskErrors.NotFound);
            return Result.Success(requestTask);
        });
    }

    public async Task<Result<FarmTask>> MarkFarmTaskAsCompleted(int taskId)
    {
        return await _loggingHelper.LogOperation(
            $"MarkFarmTaskAsCompleted({taskId})",
            async () =>
        {
            var requestTask = await _unitOfWork.FarmTaskRepository.GetByIdAsync(taskId);

            if (requestTask is null)
                return Result.Failure<FarmTask>(FarmTaskErrors.NotFound);

            var taskMarkCompletedResult = requestTask.MarkAsCompleted();
            if (taskMarkCompletedResult.IsFailure)
                return Result.Failure<FarmTask>(taskMarkCompletedResult.Error);

            var updateTask = await _unitOfWork.FarmTaskRepository.UpdateAsync(requestTask);

            return Result.Success(updateTask);
        });
    }  
}
