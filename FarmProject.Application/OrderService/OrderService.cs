using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.OrderRequestService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.OrderService;

public class OrderService(
        IUnitOfWork unitOfWork,
        IOrderRequestService orderRequestService) 
    : IOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOrderRequestService _orderRequestService = orderRequestService;

    public async Task<Result<Order>> CreateOrder(int customerId, List<CreateOrderRequest> orderRequests)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
        if (customer == null)
            return Result.Failure<Order>(CustomerErrors.NotFound);

        if (orderRequests == null || !orderRequests.Any())
            return Result.Failure<Order>(OrderErrors.EmptyOrderRequests);

        var order = new Order(customerId, DateTime.UtcNow);
        var createdOrder = await _unitOfWork.OrderRepository.AddAsync(order);

        var createdOrderRequests = new List<OrderRequest>();

        foreach (var orderRequestData in orderRequests)
        {
            var orderRequestResult = await _orderRequestService.CreateOrderRequest(
                createdOrder.Id,
                orderRequestData.CageId,
                orderRequestData.Amount);

            if (orderRequestResult.IsFailure)
                return Result.Failure<Order>(orderRequestResult.Error);

            createdOrderRequests.Add(orderRequestResult.Value);
        }

        return Result.Success(createdOrder);
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
