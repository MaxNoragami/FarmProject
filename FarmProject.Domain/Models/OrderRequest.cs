using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;

namespace FarmProject.Domain.Models;
public class OrderRequest(
        int orderId,
        Cage cage,
        int amount) 
    : Entity
{
    public int OrderId { get; private set; } = orderId;
    public OffspringType OffspringType { get; private set; } = cage.OffspringType;
    public int Amount { get; private set; } = amount;
    public Cage Cage { get; private set; } = cage;
    public OrderRequestStatus OrderRequestStatus { get; private set; } 
        = OrderRequestStatus.Waiting;

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
}
