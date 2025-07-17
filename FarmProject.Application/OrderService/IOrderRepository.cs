using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.OrderService;

public interface IOrderRepository
{
    public Task<Order> AddAsync(Order order);
    public Task<Order?> GetByIdAsync(int orderId);
    public Task<PaginatedResult<Order>> GetPaginatedAsync(PaginatedRequest<OrderFilterDto> request);
    public Task<List<Order>> FindAsync(ISpecification<Order> specification);
    public Task<Order> UpdateAsync(Order order);
}
