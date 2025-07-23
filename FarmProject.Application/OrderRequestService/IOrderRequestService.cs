using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.OrderRequestService;

public interface IOrderRequestService
{
    public Task<Result<OrderRequest>> CreateOrderRequest(int orderId, int cageId, int amount);
    public Task<Result<OrderRequest>> GetOrderRequestById(int orderRequestId);
    public Task<Result<PaginatedResult<OrderRequest>>> GetPaginatedOrderRequests(
        PaginatedRequest<OrderRequestFilterDto> request);
    public Task<Result<OrderRequest>> UpdateOrderRequestStatus(int orderRequestId, OrderRequestStatus status);
}
