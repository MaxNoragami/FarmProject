using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Validators;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.OrderService;

public class ValidationOrderService(
        IOrderService inner,
        ValidationHelper validationHelper) 
    : IOrderService
{
    private readonly IOrderService _inner = inner;
    private readonly ValidationHelper _validationHelper = validationHelper;

    public Task<Result<Order>> CreateOrder(int customerId, List<OrderRequest> orderRequests)
        => _validationHelper.ValidateAndExecute(
            new CreateOrderParam(customerId, orderRequests),
            () => _inner.CreateOrder(customerId, orderRequests));

    public Task<Result<Order>> GetOrderById(int orderId)
        => _inner.GetOrderById(orderId);

    public Task<Result<PaginatedResult<Order>>> GetPaginatedOrders(PaginatedRequest<OrderFilterDto> request)
        => _validationHelper.ValidateAndExecute(
            new PaginatedRequestParam<OrderFilterDto>(request),
            () => _inner.GetPaginatedOrders(request));
}

public record CreateOrderParam(int CustomerId, List<OrderRequest> OrderRequests);