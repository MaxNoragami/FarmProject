using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models.SortConfigs;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;
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

    public async Task<List<BreedingRabbit>> FindAsync(ISpecification<BreedingRabbit> specification)
        => await _context.BreedingRabbits
            .Where(specification.ToExpression())
            .ToListAsync();

    public async Task<BreedingRabbit?> GetByIdAsync(int breedingRabbitId)
        => await _context.BreedingRabbits
            .FirstOrDefaultAsync(r => r.Id == breedingRabbitId);

    public async Task<PaginatedResult<BreedingRabbit>> GetPaginatedAsync(PaginatedRequest<BreedingRabbitFilterDto> request)
    {
        var query = _context.BreedingRabbits
            .AsQueryable();

        if (request.Filter != null)
            query = query.ApplyFilter(request.Filter);

        var sortOrders = request.Sort?.ToSortOrders(BreedingRabbitSortingFields.AllowedSortFields)
            ?? new List<SortOrder> { new SortOrder { PropertyName = "Id", Direction = SortDirection.Ascending } };
        query = query.ApplySorting(sortOrders, BreedingRabbitSortingFields.PropertyPaths);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var result = new PaginatedResult<BreedingRabbit>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return result;
    }

    public async Task<bool> IsNameUsedAsync(string name, CancellationToken cancellationToken = default)
        => await _context.BreedingRabbits
            .AnyAsync(br => br.Name == name, cancellationToken);

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
