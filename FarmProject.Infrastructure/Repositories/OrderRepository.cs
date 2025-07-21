using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models.SortConfigs;
using FarmProject.Application.OrderService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class OrderRepository(
        FarmDbContext context) 
    : IOrderRepository
{
    private readonly FarmDbContext _context = context;

    public async Task<Order> AddAsync(Order order)
    {
        _context.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<List<Order>> FindAsync(ISpecification<Order> specification)
        => await _context.Orders
            .Where(specification.ToExpression())
            .ToListAsync();

    public async Task<Order?> GetByIdAsync(int orderId)
        => await _context.Orders
            .Include(o => o.OrderRequests)
                .ThenInclude(or => or.Cage)
            .FirstOrDefaultAsync(o => o.Id == orderId);

    public async Task<PaginatedResult<Order>> GetPaginatedAsync(PaginatedRequest<OrderFilterDto> request)
    {
        var query = _context.Orders
            .AsQueryable();

        if (request.Filter != null)
            query = query.ApplyFilter(request.Filter);

        var sortOrders = request.Sort?.ToSortOrders(OrderSortingFields.AllowedSortFields)
            ?? new List<SortOrder> { new SortOrder { PropertyName = "Id", Direction = SortDirection.Ascending } };
        query = query.ApplySorting(sortOrders, OrderSortingFields.PropertyPaths);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var result = new PaginatedResult<Order>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return result;
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }
}
