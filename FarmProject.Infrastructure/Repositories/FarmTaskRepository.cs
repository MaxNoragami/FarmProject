using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models.SortConfigs;
using FarmProject.Application.FarmTaskService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class FarmTaskRepository(FarmDbContext context) : IFarmTaskRepository
{
    public readonly FarmDbContext _context = context;

    public async Task<FarmTask> AddAsync(FarmTask farmTask)
    {
        _context.FarmTasks.Add(farmTask);
        await _context.SaveChangesAsync();
        return farmTask;
    }

    public async Task<List<FarmTask>> FindAsync(ISpecification<FarmTask> specification)
        => await _context.FarmTasks
            .Where(specification.ToExpression())
            .ToListAsync();

    public async Task<List<FarmTask>> GetAllAsync()
        => await _context.FarmTasks.ToListAsync();

    public async Task<FarmTask?> GetByIdAsync(int farmTaskId)
        => await _context.FarmTasks
            .FirstOrDefaultAsync(ft => ft.Id == farmTaskId);

    public async Task<PaginatedResult<FarmTask>> GetPaginatedAsync(PaginatedRequest<FarmTaskFilterDto> request)
    {
        var query = _context.FarmTasks
            .AsQueryable();

        if (request.Filter != null)
            query = query.ApplyFilter(request.Filter);

        var sortOrders = request.Sort?.ToSortOrders(FarmTaskSortingFields.AllowedSortFields)
            ?? new List<SortOrder> { new SortOrder { PropertyName = "Id", Direction = SortDirection.Ascending } };
        query = query.ApplySorting(sortOrders, FarmTaskSortingFields.PropertyPaths);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var result = new PaginatedResult<FarmTask>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return result;
    }

    public async Task RemoveAsync(FarmTask farmTask)
    {
        var toDelete = await _context.FarmTasks
            .FirstOrDefaultAsync(ft => ft.Id == farmTask.Id);

        if (toDelete != null)
        {
            _context.FarmTasks.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<FarmTask> UpdateAsync(FarmTask farmTask)
    {
        _context.FarmTasks.Update(farmTask);
        await _context.SaveChangesAsync();
        return farmTask;
    }
}
