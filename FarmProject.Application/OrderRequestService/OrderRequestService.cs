using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.OrderRequestService;

public class OrderRequestService(
        IUnitOfWork unitOfWork) 
    : IOrderRequestService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<OrderRequest>> CreateOrderRequest(int orderId, int cageId, int amount)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<OrderRequest>(CageErrors.NotFound);

        cage.UpdateSacrificableStatus();

        if (cage.OffspringCount <= 0)
            return Result.Failure<OrderRequest>(CageErrors.NoOffspring);

        if (!cage.IsSacrificable)
            return Result.Failure<OrderRequest>(CageErrors.NotSacrificable);

        var availableOffspring = cage.OffspringCount - cage.ReservedOffspringCount;
        if (amount > availableOffspring)
            return Result.Failure<OrderRequest>(CageErrors.InsufficientOffspring);

        var reserveResult = cage.ReserveOffspring(amount);
        if (reserveResult.IsFailure)
            return Result.Failure<OrderRequest>(reserveResult.Error);

        var orderRequest = new OrderRequest(orderId, cage, amount);

        await _unitOfWork.CageRepository.UpdateAsync(cage);
        var createdOrderRequest = await _unitOfWork.OrderRequestRepository.AddAsync(orderRequest);

        return Result.Success(createdOrderRequest);
    }

    public async Task<Result<OrderRequest>> GetOrderRequestById(int orderRequestId)
    {
        var orderRequest = await _unitOfWork.OrderRequestRepository.GetByIdAsync(orderRequestId);
        if (orderRequest == null)
            return Result.Failure<OrderRequest>(OrderRequestErrors.NotFound);

        return Result.Success(orderRequest);
    }

    public async Task<Result<PaginatedResult<OrderRequest>>> GetPaginatedOrderRequests(PaginatedRequest<OrderRequestFilterDto> request)
    {
        var orderRequests = await _unitOfWork.OrderRequestRepository.GetPaginatedAsync(request);

        return Result.Success(orderRequests);
    }

    public async Task<Result<OrderRequest>> UpdateOrderRequestStatus(int orderRequestId, OrderRequestStatus status)
    {
        var orderRequest = await _unitOfWork.OrderRequestRepository.GetByIdAsync(orderRequestId);
        if (orderRequest == null)
            return Result.Failure<OrderRequest>(OrderRequestErrors.NotFound);

        var result = orderRequest.UpdateOrderRequestStatus(status);
        if (result.IsFailure)
            return Result.Failure<OrderRequest>(result.Error);
        
        await _unitOfWork.OrderRequestRepository.UpdateAsync(orderRequest);
        return Result.Success(orderRequest);
    }
}
