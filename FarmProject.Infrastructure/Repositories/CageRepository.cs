using FarmProject.Application.CageService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models.SortConfigs;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class CageRepository(FarmDbContext context) : ICageRepository
{
    private readonly FarmDbContext _context = context;

    public async Task<Cage> AddAsync(Cage cage)
    {
        _context.Add(cage);
        await _context.SaveChangesAsync();
        return cage;
    }

    public async Task<List<Cage>> FindAsync(ISpecification<Cage> specification)
        => await _context.Cages
            .Include(C => C.BreedingRabbit)
            .Where(specification.ToExpression())
            .ToListAsync();

    public async Task<Cage?> GetByIdAsync(int cageId)
        => await _context.Cages
            .Include(c => c.BreedingRabbit)
            .FirstOrDefaultAsync(c => c.Id == cageId);

    public async Task<PaginatedResult<Cage>> GetPaginatedAsync(
        PaginatedRequest<CageFilterDto> request)
    {
        var query = _context.Cages
            .Include(c => c.BreedingRabbit)
            .AsQueryable();

        if (request.Filter != null)
            query = query.ApplyFilter(request.Filter);

        var sortOrders = request.Sort?.ToSortOrders(CageSortingFields.AllowedSortFields)
            ?? new List<SortOrder> { new SortOrder { PropertyName = "Id", Direction = SortDirection.Ascending } };
        query = query.ApplySorting(sortOrders, CageSortingFields.PropertyPaths);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var result = new PaginatedResult<Cage>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return result;
    }

    public async Task<Cage> UpdateAsync(Cage cage)
    {
        _context.Cages.Update(cage);
        await _context.SaveChangesAsync();
        return cage;
    }
}
