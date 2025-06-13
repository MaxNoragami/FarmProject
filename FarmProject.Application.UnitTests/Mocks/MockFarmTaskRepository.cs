using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.FarmTaskService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.UnitTests.Mocks;

public class MockFarmTaskRepository : IFarmTaskRepository
{
    private readonly List<FarmTask> _tasks = [];
    private int _nextId = 1;

    public Task<FarmTask> AddAsync(FarmTask task)
    {
        if (task.Id == 0)
            task.Id = _nextId++;

        _tasks.Add(task);
        return Task.FromResult(task);
    }

    public Task<List<FarmTask>> FindAsync(ISpecification<FarmTask> specification)
    {
        var expression = specification.ToExpression();
        var compiledExpression = expression.Compile();
        var result = _tasks.Where(compiledExpression).ToList();
        return Task.FromResult(result);
    }

    public Task<FarmTask?> GetByIdAsync(int taskId)
        => Task.FromResult(_tasks.FirstOrDefault(t => t.Id == taskId));

    public Task<PaginatedResult<FarmTask>> GetPaginatedAsync(PaginatedRequest<FarmTaskFilterDto> request)
    {
        IEnumerable<FarmTask> query = _tasks;

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var result = new PaginatedResult<FarmTask>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return Task.FromResult(result);
    }

    public Task RemoveAsync(FarmTask task)
    {
        _tasks.Remove(task);
        return Task.CompletedTask;
    }

    public Task<FarmTask> UpdateAsync(FarmTask task)
    {
        var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
        if (existingTask != null)
        {
            _tasks.Remove(existingTask);
            _tasks.Add(task);
        }
        return Task.FromResult(task);
    }
}
