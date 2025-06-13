using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models.SortConfigs;
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

    public async Task<Pair?> GetByIdAsync(int pairId)
        => await _context.Pairs
            .Include(p => p.FemaleRabbit)
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

    public async Task<Pair?> GetMostRecentPairByBreedingRabbitIdsAsync(int breedingRabbitId, int maleRabbitId)
    {
        return await _context.Pairs
            .Where(p => p.MaleRabbitId == maleRabbitId && p.FemaleRabbit!.Id == breedingRabbitId)
            .OrderByDescending(p => p.StartDate)
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedResult<Pair>> GetPaginatedAsync(PaginatedRequest<PairFilterDto> request)
    {
        var query = _context.Pairs
            .Include(p => p.FemaleRabbit)
            .AsQueryable();

        if (request.Filter != null)
            query = query.ApplyFilter(request.Filter);

        var sortOrders = request.Sort?.ToSortOrders(PairSortingFields.AllowedSortFields)
            ?? new List<SortOrder> { new SortOrder { PropertyName = "Id", Direction = SortDirection.Ascending } };
        query = query.ApplySorting(sortOrders, PairSortingFields.PropertyPaths);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var result = new PaginatedResult<Pair>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return result;
    }
}
