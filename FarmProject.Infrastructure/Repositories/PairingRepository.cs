using FarmProject.Application.PairingService;
using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class PairingRepository(FarmDbContext context) : IPairingRepository
{
    private readonly FarmDbContext _context = context;

    public async Task<Pair> AddAsync(Pair pair)
    {
        _context.Pairs.Add(pair);
        await _context.SaveChangesAsync();
        return pair;
    }

    public async Task<List<Pair>> GetAllAsync()
        => await _context.Pairs
            .Include(p => p.MaleBreedingRabbit)
            .Include(p => p.FemaleBreedingRabbit)
            .ToListAsync();

    public async Task<Pair?> GetByIdAsync(int pairId)
        => await _context.Pairs
            .Include(p => p.MaleBreedingRabbit)
            .Include(p => p.FemaleBreedingRabbit)
            .FirstOrDefaultAsync(p => p.Id == pairId);

    public async Task RemoveAsync(Pair pair)
    {
        var toDelete = await _context.Pairs
            .FirstOrDefaultAsync(p => p.Id == pair.Id);

        if (toDelete != null)
        {
            _context.Pairs.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Pair> UpdateAsync(Pair pair)
    {
        _context.Pairs.Update(pair);
        await _context.SaveChangesAsync();
        return pair;
    }

    public async Task<Pair?> GetMostRecentPairByBreedingRabbitIdsAsync(int breedingRabbitId1, int breedingRabbitId2)
    {
        return await _context.Pairs
            .Where(p =>
                (p.MaleBreedingRabbit.Id == breedingRabbitId1 && p.FemaleBreedingRabbit.Id == breedingRabbitId2) ||
                (p.MaleBreedingRabbit.Id == breedingRabbitId2 && p.FemaleBreedingRabbit.Id == breedingRabbitId1))
            .OrderByDescending(p => p.StartDate)
            .FirstOrDefaultAsync();
    }

}
