using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Validators;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.OrderRequestService;

public class ValidationOrderRequestService(
        IOrderRequestService inner,
        ValidationHelper validationHelper) 
    : IOrderRequestService
{
    private readonly IOrderRequestService _inner = inner;
    private readonly ValidationHelper _validationHelper = validationHelper;

    public Task<Result<OrderRequest>> CreateOrderRequest(int orderId, int cageId, int amount)
        => _validationHelper.ValidateAndExecute(
            new CreateOrderRequestParam(orderId, cageId, amount),
                () => _inner.CreateOrderRequest(orderId, cageId, amount));

    public Task<Result<OrderRequest>> GetOrderRequestById(int orderRequestId)
        => _inner.GetOrderRequestById(orderRequestId);

    public Task<Result<PaginatedResult<OrderRequest>>> GetPaginatedOrderRequests(PaginatedRequest<OrderRequestFilterDto> request)
        => _validationHelper.ValidateAndExecute(
            new PaginatedRequestParam<OrderRequestFilterDto>(request),
                () => _inner.GetPaginatedOrderRequests(request));

    public Task<Result<OrderRequest>> UpdateOrderRequestStatus(int orderRequestId, OrderRequestStatus status)
        => _validationHelper.ValidateAndExecute(
            new UpdateOrderRequestStatusParam(orderRequestId, status),
                () => _inner.UpdateOrderRequestStatus(orderRequestId, status));
}

public record CreateOrderRequestParam(int OrderId, int CageId, int Amount);
public record UpdateOrderRequestStatusParam(int OrderRequestId, OrderRequestStatus Status);