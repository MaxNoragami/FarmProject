using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
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

    public Task<BreedingRabbit?> GetByIdAsync(int breedingRabbitId)
        => Task.FromResult(_breedingRabbits.FirstOrDefault(r => r.Id == breedingRabbitId));

    public Task<PaginatedResult<BreedingRabbit>> GetPaginatedAsync(PaginatedRequest<BreedingRabbitFilterDto> request)
    {
        IEnumerable<BreedingRabbit> query = _breedingRabbits;

        if (request.Filter != null)
        {
            if (!string.IsNullOrEmpty(request.Filter.Name))
            {
                query = query.Where(r => r.Name.Contains(request.Filter.Name));
            }
        }

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var result = new PaginatedResult<BreedingRabbit>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return Task.FromResult(result);
    }

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
