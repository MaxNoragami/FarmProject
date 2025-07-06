using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.FarmTaskService;

public class LoggingFarmTaskService(
        IFarmTaskService farmTaskService,
        LoggingHelper loggingHelper)
    : IFarmTaskService
{
    private readonly IFarmTaskService _farmTaskService = farmTaskService;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<FarmTask>> CreateFarmTask(FarmTask farmTask)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(CreateFarmTask),
                (nameof(farmTask), farmTask)
            ),
            async () =>
                await _farmTaskService.CreateFarmTask(farmTask));

    public async Task<Result<FarmTask>> GetFarmTaskById(int taskId)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetFarmTaskById),
                (nameof(taskId), taskId)
            ),
            async () =>
                await _farmTaskService.GetFarmTaskById(taskId));

    public async Task<Result<PaginatedResult<FarmTask>>> GetPaginatedFarmTasks(PaginatedRequest<FarmTaskFilterDto> request)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetPaginatedFarmTasks),
                (nameof(request), request)
            ),
            async () =>
                await _farmTaskService.GetPaginatedFarmTasks(request));

    public async Task<Result<FarmTask>> MarkFarmTaskAsCompleted(
        int taskId, 
        CompleteTaskData? completeTaskData = null
    )
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(MarkFarmTaskAsCompleted),
                (nameof(taskId), taskId),
                (nameof(completeTaskData), completeTaskData)
            ),
            async () =>
                await _farmTaskService.MarkFarmTaskAsCompleted(taskId, completeTaskData));
}
