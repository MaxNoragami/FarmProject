using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.UnitTests.Mocks;

public class MockBreedingRabbitRepository : IBreedingRabbitRepository
{
    private readonly List<BreedingRabbit> _breedingRabbits = [];
    private int _nextId = 1;

    public Task<BreedingRabbit> AddAsync(BreedingRabbit breedingRabbit)
    {
        if (breedingRabbit.Id == 0)
            breedingRabbit.Id = _nextId++;

        _breedingRabbits.Add(breedingRabbit);
        return Task.FromResult(breedingRabbit);
    }

    public Task<List<BreedingRabbit>> FindAsync(ISpecification<BreedingRabbit> specification)
    {
        var expression = specification.ToExpression();
        var compiledExpression = expression.Compile();
        var result = _breedingRabbits.Where(compiledExpression).ToList();
        return Task.FromResult(result);
    }

    public Task<List<BreedingRabbit>> GetAllAsync()
        => Task.FromResult(_breedingRabbits.ToList());

    public Task<BreedingRabbit?> GetByIdAsync(int breedingRabbitId)
        => Task.FromResult(_breedingRabbits.FirstOrDefault(r => r.Id == breedingRabbitId));

    public Task RemoveAsync(BreedingRabbit breedingRabbit)
    {
        _breedingRabbits.Remove(breedingRabbit);
        return Task.CompletedTask;
    }

    public Task<BreedingRabbit> UpdateAsync(BreedingRabbit breedingRabbit)
    {
        var existingRabbit = _breedingRabbits.FirstOrDefault(r => r.Id == breedingRabbit.Id);
        if (existingRabbit != null)
        {
            _breedingRabbits.Remove(existingRabbit);
            _breedingRabbits.Add(breedingRabbit);
        }
        return Task.FromResult(breedingRabbit);
    }
}
