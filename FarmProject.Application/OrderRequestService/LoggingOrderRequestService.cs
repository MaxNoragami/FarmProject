using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.OrderRequestService;

public class LoggingOrderRequestService(
        IOrderRequestService orderRequestService,
        LoggingHelper loggingHelper) 
    : IOrderRequestService
{
    private readonly IOrderRequestService _orderRequestService = orderRequestService;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<OrderRequest>> CreateOrderRequest(
        int orderId, 
        int cageId, 
        int amount
    )
        => await _loggingHelper.LogOperation(
                LoggingUtilities.FormatMethodCall(
                    nameof(CreateOrderRequest),
                    (nameof(orderId), orderId),
                    (nameof(cageId), cageId),
                    (nameof(amount), amount)
                ),
                async () =>
                    await _orderRequestService.CreateOrderRequest(
                        orderId, cageId, amount));

    public async Task<Result<OrderRequest>> GetOrderRequestById(int orderRequestId)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetOrderRequestById),
                (nameof(orderRequestId), orderRequestId)
            ),
            async () =>
                await _orderRequestService.GetOrderRequestById(orderRequestId));

    public async Task<Result<PaginatedResult<OrderRequest>>> GetPaginatedOrderRequests(
        PaginatedRequest<OrderRequestFilterDto> request
    )
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetPaginatedOrderRequests),
                (nameof(request), request)
            ),
            async () =>
                await _orderRequestService.GetPaginatedOrderRequests(request));

    public async Task<Result<OrderRequest>> UpdateOrderRequestStatus(
        int orderRequestId, 
        OrderRequestStatus status
    )
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(UpdateOrderRequestStatus),
                (nameof(orderRequestId), orderRequestId),
                (nameof(status), status)
            ),
            async () =>
                await _orderRequestService.UpdateOrderRequestStatus(
                    orderRequestId, status));
}
