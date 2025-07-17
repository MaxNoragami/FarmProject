using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
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

    public async Task<Result<Order>> GetOrderById(int orderId)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
        if (order == null)
            return Result.Failure<Order>(OrderErrors.NotFound);

        return Result.Success(order);
    }

    public async Task<Result<PaginatedResult<Order>>> GetPaginatedOrders(PaginatedRequest<OrderFilterDto> request)
    {
        var orders = await _unitOfWork.OrderRepository.GetPaginatedAsync(request);

        return Result.Success(orders);
    }
}
