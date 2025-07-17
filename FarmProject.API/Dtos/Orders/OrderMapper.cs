using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.Orders;

public static class OrderMapper
{
    public static ViewOrdersDto ToViewOrdersDto(this Order order)
        => new ViewOrdersDto()
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderStatus = order.OrderStatus,
            OrderDate = order.OrderDate
        };

    public static ViewSingleOrderDto ToViewSingleOrderDto(this Order order)
        => new ViewSingleOrderDto()
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderStatus = order.OrderStatus,
            OrderDate = order.OrderDate,
            OrderRequests = order.OrderRequests
        };
}

