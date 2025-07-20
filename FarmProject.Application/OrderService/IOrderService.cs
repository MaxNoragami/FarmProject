using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.OrderService;

public interface IOrderService
{
    public Task<Result<Order>> CreateOrder(int customerId, List<CreateOrderRequest> orderRequests);
    public Task<Result<PaginatedResult<Order>>> GetPaginatedOrders(
        PaginatedRequest<OrderFilterDto> request);
    public Task<Result<Order>> GetOrderById(int orderId);
}
