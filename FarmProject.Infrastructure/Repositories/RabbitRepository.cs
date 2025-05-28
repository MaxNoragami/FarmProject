using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class RabbitRepository(FarmDbContext context) : IRabbitRepository
{
    private readonly FarmDbContext _context = context;

    public async Task<Rabbit> AddAsync(Rabbit rabbit)
    {
        _context.Rabbits.Add(rabbit);
        await _context.SaveChangesAsync();
        return rabbit;
    }

    public async Task<List<Rabbit>> GetAllAsync()
        => await _context.Rabbits.ToListAsync();

    public async Task<Rabbit?> GetByIdAsync(int rabbitId)
        => await _context.Rabbits
            .FirstOrDefaultAsync(r => r.Id == rabbitId);

    public async Task RemoveAsync(Rabbit rabbit)
    {
        var toDelete = await _context.Rabbits
            .FirstOrDefaultAsync(r => r.Id == rabbit.Id);

        if (toDelete != null)
        {
            _context.Rabbits.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Rabbit> UpdateAsync(Rabbit rabbit)
    {
        _context.Rabbits.Update(rabbit);
        await _context.SaveChangesAsync();
        return rabbit;
    }
}
