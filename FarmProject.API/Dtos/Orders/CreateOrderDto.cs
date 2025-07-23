using FarmProject.Application.OrderService;
using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.Orders;

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public List<CreateOrderRequest> OrderRequests { get; set; }
}
