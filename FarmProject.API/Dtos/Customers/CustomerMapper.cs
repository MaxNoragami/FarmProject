using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.Customers;

public static class CustomerMapper
{
    public static ViewCustomersDto ToViewCustomersDto(this Customer customer)
        => new ViewCustomersDto()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNum = customer.PhoneNum
        };

    public static ViewSingleCustomerDto ToViewSingleCustomerDto(this Customer customer)
        => new ViewSingleCustomerDto()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNum = customer.PhoneNum,
            Orders = customer.Orders
        };
}
