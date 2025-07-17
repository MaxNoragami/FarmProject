using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.Orders;

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public List<OrderRequest> OrderRequests { get; set; }
}
