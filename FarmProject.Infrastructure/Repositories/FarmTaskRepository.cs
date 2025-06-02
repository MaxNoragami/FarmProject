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
