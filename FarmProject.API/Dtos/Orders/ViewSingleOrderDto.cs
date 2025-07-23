using FarmProject.API.Dtos.OrderRequests;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
namespace FarmProject.API.Dtos.Orders;

public class ViewSingleOrderDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
    public List<ViewOrderRequestDto> OrderRequests { get; set; } = new();
}
