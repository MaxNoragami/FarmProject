using FarmProject.Application.FarmEventsService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class FarmEventRepository(FarmDbContext context) : IFarmEventRepository
{
    public readonly FarmDbContext _context = context;

    public async Task<FarmEvent> AddAsync(FarmEvent farmEvent)
    {
        _context.FarmEvents.Add(farmEvent);
        await _context.SaveChangesAsync();
        return farmEvent;
    }

    public async Task<List<FarmEvent>> FindAsync(ISpecification<FarmEvent> specification)
        => await _context.FarmEvents
            .Where(specification.ToExpression())
            .ToListAsync();

    public async Task<List<FarmEvent>> GetAllAsync()
        => await _context.FarmEvents.ToListAsync();

    public async Task<FarmEvent?> GetByIdAsync(int farmEventId)
        => await _context.FarmEvents
            .FirstOrDefaultAsync(fe => fe.Id == farmEventId);

    public async Task RemoveAsync(FarmEvent farmEvent)
    {
        var toDelete = await _context.FarmEvents
            .FirstOrDefaultAsync(fe => fe.Id == farmEvent.Id);

        if (toDelete != null)
        {
            _context.FarmEvents.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<FarmEvent> UpdateAsync(FarmEvent farmEvent)
    {
        _context.FarmEvents.Update(farmEvent);
        await _context.SaveChangesAsync();
        return farmEvent;
    }
}
