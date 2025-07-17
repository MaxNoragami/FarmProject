using FarmProject.Domain.Constants;

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
}
