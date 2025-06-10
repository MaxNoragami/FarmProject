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
                cage.AssignBreedingRabbit(new BreedingRabbit($"Breeding Rabbit {i}")
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

    public async Task<Cage> SeedCageWithAssignedRabbit()
    {
        // Get next available IDs
        int nextCageId = await _context.Cages.AnyAsync()
            ? await _context.Cages.MaxAsync(c => c.Id) + 1
            : 1;

        int nextRabbitId = await _context.BreedingRabbits.AnyAsync()
            ? await _context.BreedingRabbits.MaxAsync(r => r.Id) + 1
            : 1;

        // Create cage with the next available ID
        var cage = new Cage($"Cage {nextCageId}")
        {
            Id = nextCageId
        };

        // Create rabbit with the next available ID
        var rabbit = new BreedingRabbit($"Breeding Rabbit {nextRabbitId}")
        {
            Id = nextRabbitId
        };

        // Assign rabbit to cage
        var result = cage.AssignBreedingRabbit(rabbit);
        if (result.IsSuccess)
        {
            _context.Cages.Add(cage);
            rabbit.CageId = cage.Id;
            _context.BreedingRabbits.Add(rabbit);
            await _context.SaveChangesAsync();
        }

        return cage;
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
        var rabbits = await SeedBreedingRabbits(numberOfPairs);
        var pairs = new List<Pair>();

        for (int i = 0; i < numberOfPairs; i++)
        {
            var maleId = 1000 + i + 1;
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
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }
}
