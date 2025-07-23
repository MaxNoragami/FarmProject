using FarmProject.API.Dtos.Orders;
using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.Customers;

public class ViewSingleCustomerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNum { get; set; }
    public List<ViewOrdersDto> Orders { get; set; } = new();
}
