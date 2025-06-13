using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.FarmTaskService;

public interface IFarmTaskRepository
{
    public Task<FarmTask> AddAsync(FarmTask farmTask);
    public Task<FarmTask?> GetByIdAsync(int farmTaskId);
    public Task<List<FarmTask>> GetAllAsync();
    public Task<PaginatedResult<FarmTask>> GetPaginatedAsync(PaginatedRequest<FarmTaskFilterDto> request);
    public Task<List<FarmTask>> FindAsync(ISpecification<FarmTask> specification);
    public Task<FarmTask> UpdateAsync(FarmTask farmTask);
    public Task RemoveAsync(FarmTask farmTask);
}
