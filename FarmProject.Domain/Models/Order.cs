using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;

namespace FarmProject.Domain.Models;

public class Order(
        int customerId,
        DateTime orderDate) 
    : Entity
{
    public int CustomerId { get; private set; } = customerId;
    public OrderStatus OrderStatus { get; private set; } = OrderStatus.Processing;
    public DateTime OrderDate { get; private set; } = orderDate;
    public List<OrderRequest> OrderRequests { get; private set; } = new();

    public Result UpdateStatusBasedOnOrderRequests()
    {
        if (OrderRequests.All(or => or.OrderRequestStatus == OrderRequestStatus.Completed))
            OrderStatus = OrderStatus.Completed;
        else if (OrderRequests.Any(or => or.OrderRequestStatus == OrderRequestStatus.Failed))
            OrderStatus = OrderStatus.Failed;
        else
            OrderStatus = OrderStatus.Processing;

        return Result.Success();
    }
}
