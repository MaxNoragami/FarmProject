using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.OrderRequests;

public static class OrderRequestMapper
{
    public static ViewOrderRequestDto ToViewOrderRequestDto(this OrderRequest orderRequest)
        => new ViewOrderRequestDto()
            {
                Id = orderRequest.Id,
                OrderId = orderRequest.OrderId,
                OffspringType = orderRequest.OffspringType,
                Amount = orderRequest.Amount,
                SacrificedAmount = orderRequest.SacrificedAmount,
                CageId = orderRequest.Cage.Id,
                OrderRequestStatus = orderRequest.OrderRequestStatus
            };
}
