using FarmProject.Application.CageService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
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

    public Task<Cage?> GetByIdAsync(int cageId)
        => Task.FromResult(_cages.FirstOrDefault(c => c.Id == cageId));

    public Task<PaginatedResult<Cage>> GetPaginatedAsync(PaginatedRequest<CageFilterDto> request)
    {
        IEnumerable<Cage> query = _cages;

        if (request.Filter != null)
        {
            if (!string.IsNullOrEmpty(request.Filter.Name))
                query = query.Where(c => c.Name.Contains(request.Filter.Name));

            if (request.Filter.IsOccupied.HasValue)
            {
                bool isOccupied = request.Filter.IsOccupied.Value;
               
                query = query.Where(c => 
                    (c.BreedingRabbit != null || c.OffspringCount > 0) == isOccupied);
            }

            if (request.Filter.OffspringType.HasValue)
                query = query.Where(c => c.OffspringType == request.Filter.OffspringType.Value);
        }

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var result = new PaginatedResult<Cage>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return Task.FromResult(result);
    }

    public Task<bool> IsNameUsedAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

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
