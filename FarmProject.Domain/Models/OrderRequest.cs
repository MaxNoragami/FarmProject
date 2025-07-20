using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;

namespace FarmProject.Domain.Models;
public class OrderRequest : Entity
{
    public int OrderId { get; private set; }
    public OffspringType OffspringType { get; private set; }
    public int Amount { get; private set; }
    public Cage Cage { get; private set; }
    public OrderRequestStatus OrderRequestStatus { get; private set; }

    private OrderRequest() { }

    public OrderRequest(int orderId, Cage cage, int amount)
    {
        OrderId = orderId;
        Cage = cage;
        OffspringType = cage.OffspringType;
        Amount = amount;
        OrderRequestStatus = OrderRequestStatus.Waiting;
    }

    public Result ProcessSacrifice(int sacrificeAmount)
    {
        if (sacrificeAmount > Amount || sacrificeAmount <= 0)
            return Result.Failure(OrderRequestErrors.InvalidAmount);

        var result = Cage.RemoveReservedOffspring(sacrificeAmount);
        if (result.IsFailure)
            return Result.Failure(result.Error);

        if (Cage.ReservedOffspringCount == 0)
            OrderRequestStatus = OrderRequestStatus.Completed;

        return Result.Success();
    }

    public Result UpdateOrderRequestStatus(OrderRequestStatus newOrderRequestStatus)
    {
        OrderRequestStatus = newOrderRequestStatus;
        return Result.Success();
    }
}
