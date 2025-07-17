using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.OrderService;

public class OrderService(
        IUnitOfWork unitOfWork) 
    : IOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public Task<Result<Order>> CreateOrder(int customerId, List<OrderRequest> orderRequests)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Order>> GetOrderById(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<PaginatedResult<Order>>> GetPaginatedOrders(PaginatedRequest<OrderFilterDto> request)
    {
        throw new NotImplementedException();
    }
}
