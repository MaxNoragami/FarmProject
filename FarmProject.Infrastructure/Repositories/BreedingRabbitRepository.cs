using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class BreedingRabbitRepository(FarmDbContext context) : IBreedingRabbitRepository
{
    private readonly FarmDbContext _context = context;

    public async Task<BreedingRabbit> AddAsync(BreedingRabbit breedingRabbit)
    {
        _context.BreedingRabbits.Add(breedingRabbit);
        await _context.SaveChangesAsync();
        return breedingRabbit;
    }

    public async Task<List<BreedingRabbit>> GetAllAsync()
        => await _context.BreedingRabbits.ToListAsync();

    public async Task<BreedingRabbit?> GetByIdAsync(int breedingRabbitId)
        => await _context.BreedingRabbits
            .FirstOrDefaultAsync(r => r.Id == breedingRabbitId);

    public async Task RemoveAsync(BreedingRabbit breedingRabbit)
    {
        var toDelete = await _context.BreedingRabbits
            .FirstOrDefaultAsync(r => r.Id == breedingRabbit.Id);

        if (toDelete != null)
        {
            _context.BreedingRabbits.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<BreedingRabbit> UpdateAsync(BreedingRabbit breedingRabbit)
    {
        _context.BreedingRabbits.Update(breedingRabbit);
        await _context.SaveChangesAsync();
        return breedingRabbit;
    }
}
