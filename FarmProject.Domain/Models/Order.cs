namespace FarmProject.Domain.Models;

public class Order(
        int customerId,
        DateTime orderDate) 
    : Entity
{
    public int CustomerId { get; private set; } = customerId;
    public DateTime OrderDate { get; private set; } = orderDate;
    public List<OrderRequest> OrderRequests { get; private set; } = new();
}
