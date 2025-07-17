using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models.SortConfigs;
using FarmProject.Application.SacrificationService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class SacrificationRepository(
        FarmDbContext context) 
    : ISacrificationRepository
{
    private readonly FarmDbContext _context = context;

    public async Task<Sacrification> AddAsync(Sacrification sacrification)
    {
        _context.Add(sacrification);
        await _context.SaveChangesAsync();
        return sacrification;
    }

    public async Task<List<Sacrification>> FindAsync(ISpecification<Sacrification> specification)
        => await _context.Sacrifications
            .Where(specification.ToExpression())
            .ToListAsync();

    public async Task<Sacrification?> GetByIdAsync(int sacrificationId)
        => await _context.Sacrifications
            .FirstOrDefaultAsync(s => s.Id == sacrificationId);

    public async Task<PaginatedResult<Sacrification>> GetPaginatedAsync(PaginatedRequest<SacrificationFilterDto> request)
    {
        var query = _context.Sacrifications
            .AsQueryable();

        if (request.Filter != null)
            query = query.ApplyFilter(request.Filter);

        var sortOrders = request.Sort?.ToSortOrders(SacrificationSortingFields.AllowedSortFields)
            ?? new List<SortOrder> { new SortOrder { PropertyName = "Id", Direction = SortDirection.Ascending } };
        query = query.ApplySorting(sortOrders, SacrificationSortingFields.PropertyPaths);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var result = new PaginatedResult<Sacrification>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return result;
    }

    public async Task<Sacrification> UpdateAsync(Sacrification sacrification)
    {
        _context.Sacrifications.Update(sacrification);
        await _context.SaveChangesAsync();
        return sacrification;
    }
}
