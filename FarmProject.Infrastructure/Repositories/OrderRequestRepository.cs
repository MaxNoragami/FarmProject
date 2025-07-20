using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models.SortConfigs;
using FarmProject.Application.OrderRequestService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class OrderRequestRepository(
        FarmDbContext context) 
    : IOrderRequestRepository
{
    private readonly FarmDbContext _context = context;

    public async Task<OrderRequest> AddAsync(OrderRequest orderRequest)
    {
        _context.Add(orderRequest);
        await _context.SaveChangesAsync();
        return orderRequest;
    }

    public async Task<List<OrderRequest>> FindAsync(ISpecification<OrderRequest> specification)
        => await _context.OrderRequests
            .Where(specification.ToExpression())
            .ToListAsync();

    public async Task<OrderRequest?> GetByIdAsync(int orderRequestId)
        => await _context.OrderRequests
            .FirstOrDefaultAsync(or => or.Id == orderRequestId);

    public async Task<PaginatedResult<OrderRequest>> GetPaginatedAsync(PaginatedRequest<OrderRequestFilterDto> request)
    {
        var query = _context.OrderRequests
            .AsQueryable();

        if (request.Filter != null)
            query = query.ApplyFilter(request.Filter);

        var sortOrders = request.Sort?.ToSortOrders(OrderRequestSortingFields.AllowedSortFields)
            ?? new List<SortOrder> { new SortOrder { PropertyName = "Id", Direction = SortDirection.Ascending } };
        query = query.ApplySorting(sortOrders, OrderRequestSortingFields.PropertyPaths);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var result = new PaginatedResult<OrderRequest>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return result;
    }

    public async Task<OrderRequest> UpdateAsync(OrderRequest orderRequest)
    {
        _context.OrderRequests.Update(orderRequest);
        await _context.SaveChangesAsync();
        return orderRequest;
    }
}
