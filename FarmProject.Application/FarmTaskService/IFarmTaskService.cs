using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.FarmTaskService;

public interface IFarmTaskService
{
    public Task<Result<FarmTask>> CreateFarmTask(FarmTask farmTask);
    public Task<Result<PaginatedResult<FarmTask>>> GetPaginatedFarmTasks(PaginatedRequest<FarmTaskFilterDto> request);
    public Task<Result<FarmTask>> GetFarmTaskById(int taskId);
    public Task<Result<FarmTask>> MarkFarmTaskAsCompleted(int taskId);
}
