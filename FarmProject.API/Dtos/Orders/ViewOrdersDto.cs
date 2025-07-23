using FarmProject.Domain.Constants;

namespace FarmProject.API.Dtos.Orders;

public class ViewOrdersDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
}
