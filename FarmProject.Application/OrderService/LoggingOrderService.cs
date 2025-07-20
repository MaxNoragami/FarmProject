using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.OrderService;

public class LoggingOrderService(
        IOrderService orderService,
        LoggingHelper loggingHelper) 
    : IOrderService
{
    private readonly IOrderService _orderService = orderService;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<Order>> CreateOrder(int customerId, List<CreateOrderRequest> orderRequests)
        => await _loggingHelper.LogOperation(
                LoggingUtilities.FormatMethodCall(
                    nameof(CreateOrder),
                    (nameof(customerId), customerId),
                    (nameof(orderRequests), orderRequests)
                ),
                async () =>
                    await _orderService.CreateOrder(customerId, orderRequests));

    public async Task<Result<Order>> GetOrderById(int orderId)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetOrderById),
                (nameof(orderId), orderId)
            ),
            async () =>
                await _orderService.GetOrderById(orderId));

    public async Task<Result<PaginatedResult<Order>>> GetPaginatedOrders(
        PaginatedRequest<OrderFilterDto> request
    )
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetPaginatedOrders),
                (nameof(request), request)
            ),
            async () =>
                await _orderService.GetPaginatedOrders(request));
}
