using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.PairingService;
using FarmProject.Domain.Models;

namespace FarmProject.Application.UnitTests.Mocks;

public class MockPairingRepository : IPairingRepository
{
    private readonly List<Pair> _pairs = [];
    private int _nextId = 1;

    public Task<Pair> AddAsync(Pair pair)
    {
        if (pair.Id == 0)
            pair.Id = _nextId++;

        _pairs.Add(pair);
        return Task.FromResult(pair);
    }

    public Task<Pair?> GetByIdAsync(int pairId)
        => Task.FromResult(_pairs.FirstOrDefault(p => p.Id == pairId));

    public Task<Pair?> GetMostRecentPairByBreedingRabbitIdsAsync(int breedingRabbitId1, int breedingRabbitId2)
    {
        return Task.FromResult(_pairs.FirstOrDefault(p =>
            p.FemaleRabbit?.Id == breedingRabbitId1 && p.MaleRabbitId == breedingRabbitId2));
    }

    public Task<PaginatedResult<Pair>> GetPaginatedAsync(PaginatedRequest<PairFilterDto> request)
    {
        IEnumerable<Pair> query = _pairs;

        // Apply basic filtering if provided
        if (request.Filter != null)
        {
            if (request.Filter.MaleRabbitId.HasValue)
            {
                query = query.Where(p => p.MaleRabbitId == request.Filter.MaleRabbitId.Value);
            }

            if (request.Filter.PairingStatus.HasValue)
            {
                query = query.Where(p => p.PairingStatus == request.Filter.PairingStatus.Value);
            }
        }

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var result = new PaginatedResult<Pair>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return Task.FromResult(result);
    }

    public Task RemoveAsync(Pair pair)
    {
        _pairs.Remove(pair);
        return Task.CompletedTask;
    }

    public Task<Pair> UpdateAsync(Pair pair)
    {
        var existingPair = _pairs.FirstOrDefault(p => p.Id == pair.Id);
        if (existingPair != null)
        {
            _pairs.Remove(existingPair);
            _pairs.Add(pair);
        }
        return Task.FromResult(pair);
    }
}
