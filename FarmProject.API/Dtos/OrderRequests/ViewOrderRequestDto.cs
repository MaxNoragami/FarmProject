using FarmProject.Domain.Constants;

namespace FarmProject.API.Dtos.OrderRequests;

public class ViewOrderRequestDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public OffspringType OffspringType { get; set; }
    public int Amount { get; set; }
    public int CageId { get; set; }
    public OrderRequestStatus OrderRequestStatus { get; set; }
}
