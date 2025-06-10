using FarmProject.Domain.Models;
using FarmProject.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.API.IntegrationTests.Helpers;

public class TestDataSeeder(FarmDbContext context)
{
    private readonly FarmDbContext _context = context;

    public async Task<List<Cage>> SeedCages(int numberOfCages = 3, bool includeOccupiedCage = true)
    {
        _context.Cages.RemoveRange(_context.Cages);
        await _context.SaveChangesAsync();

        var cages = new List<Cage>();

        for (int i = 1; i <= numberOfCages; i++)
        {
            var cage = new Cage($"Cage {i}")
            {
                Id = i
            };

            if (includeOccupiedCage && i == numberOfCages)
                cage.AssignBreedingRabbit(new BreedingRabbit($"Test Rabbit {i}")
                {
                    Id = i
                });

            cages.Add(cage);
        }

        _context.Cages.AddRange(cages);
        await _context.SaveChangesAsync();
        return cages;
    }

    private async Task<List<BreedingRabbit>> SeedBreedingRabbits(int numberOfRabbits = 3)
    {
        _context.BreedingRabbits.RemoveRange(_context.BreedingRabbits);
        await _context.SaveChangesAsync();

        var rabbits = new List<BreedingRabbit>();

        for (int i = 1; i <= numberOfRabbits; i++)
        {
            var rabbit = new BreedingRabbit($"Breeding Rabbit {i}")
            {
                Id = i
            };
            rabbits.Add(rabbit);
        }

        _context.BreedingRabbits.AddRange(rabbits);
        await _context.SaveChangesAsync();
        return rabbits;
    }

    public async Task<List<Cage>> SeedCagesWithRabbits(int numberOfCages = 3)
    {
        var cages = await SeedCages(numberOfCages);

        var rabbits = await SeedBreedingRabbits(numberOfCages);

        for (int i = 0; i < numberOfCages; i++)
        {
            var cage = cages[i];
            var rabbit = rabbits[i];

            var result = cage.AssignBreedingRabbit(rabbit);
            if (result.IsSuccess)
            {
                _context.Cages.Update(cage);
                rabbit.CageId = cage.Id;
                _context.BreedingRabbits.Update(rabbit);
            }
        }

        await _context.SaveChangesAsync();
        return cages;
    }

    public async Task<List<Pair>> SeedPairs(int numberOfPairs = 2)
    {
        var rabbits = await _context.BreedingRabbits.ToListAsync();
        if (rabbits.Count < numberOfPairs)
            rabbits = await SeedBreedingRabbits(numberOfPairs);

        _context.Pairs.RemoveRange(_context.Pairs);
        await _context.SaveChangesAsync();

        var pairs = new List<Pair>();

        for (int i = 0; i < numberOfPairs; i++)
        {
            var maleId = 1000 + i;
            var pair = new Pair(maleId, rabbits[i], DateTime.UtcNow)
            {
                Id = i + 1
            };
            pairs.Add(pair);
        }

        _context.Pairs.AddRange(pairs);
        await _context.SaveChangesAsync();

        return pairs;
    }

    public async Task ClearDatabase()
    {
        _context.Pairs.RemoveRange(_context.Pairs);
        _context.FarmTasks.RemoveRange(_context.FarmTasks);
        _context.BreedingRabbits.RemoveRange(_context.BreedingRabbits);
        _context.Cages.RemoveRange(_context.Cages);
        await _context.SaveChangesAsync();
    }
}
