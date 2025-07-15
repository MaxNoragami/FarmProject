using FarmProject.Domain.Constants;

namespace FarmProject.Domain.Models;
public class OrderRequest(
        int orderId,
        int cageId,
        OffspringType type,
        int amount) 
    : Entity
{
    public int OrderId { get; private set; } = orderId;
    public OffspringType Type { get; private set; } = type;
    public int Amount { get; private set; } = amount;
    public int CageId { get; private set; } = cageId;
}
