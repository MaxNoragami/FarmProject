using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Validators;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.FarmTaskService;

public class ValidationFarmTaskService(
        IFarmTaskService inner,
        ValidationHelper validationHelper)
    : IFarmTaskService
{
    private readonly IFarmTaskService _inner = inner;
    private readonly ValidationHelper _validationHelper = validationHelper;

    public Task<Result<FarmTask>> CreateFarmTask(FarmTask farmTask)
        => _inner.CreateFarmTask(farmTask);

    public Task<Result<FarmTask>> GetFarmTaskById(int taskId)
        => _inner.GetFarmTaskById(taskId);

    public Task<Result<PaginatedResult<FarmTask>>> GetPaginatedFarmTasks(PaginatedRequest<FarmTaskFilterDto> request)
        => _validationHelper.ValidateAndExecute(
                new PaginatedRequestParam<FarmTaskFilterDto>(request),
                () => _inner.GetPaginatedFarmTasks(request));

    public Task<Result<FarmTask>> MarkFarmTaskAsCompleted(int taskId)
        => _inner.MarkFarmTaskAsCompleted(taskId);
}
