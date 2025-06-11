using FarmProject.Application.CageService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.UnitTests.Mocks;

public class MockCageRepository : ICageRepository
{
    private readonly List<Cage> _cages = [];
    private int _nextId = 1;

    public Task<Cage> AddAsync(Cage cage)
    {
        if (cage.Id == 0)
            cage.Id = _nextId++;

        _cages.Add(cage);
        return Task.FromResult(cage);
    }

    public Task<List<Cage>> FindAsync(ISpecification<Cage> specification)
    {
        var expression = specification.ToExpression();
        var compiledExpression = expression.Compile();
        var result = _cages.Where(compiledExpression).ToList();
        return Task.FromResult(result);
    }

    public Task<List<Cage>> GetAllAsync()
        => Task.FromResult(_cages.ToList());

    public Task<Cage?> GetByIdAsync(int cageId)
        => Task.FromResult(_cages.FirstOrDefault(c => c.Id == cageId));

    public Task RemoveAsync(Cage cage)
    {
        _cages.Remove(cage);
        return Task.CompletedTask;
    }

    public Task<Cage> UpdateAsync(Cage cage)
    {
        var existingCage = _cages.FirstOrDefault(c => c.Id == cage.Id);
        if (existingCage != null)
        {
            _cages.Remove(existingCage);
            _cages.Add(cage);
        }
        return Task.FromResult(cage);
    }
}
