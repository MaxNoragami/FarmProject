using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.Customers;

public static class CustomerMapper
{
    public static ViewCustomerDto ToViewCustomerDto(this Customer customer)
        => new ViewCustomerDto()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNum = customer.PhoneNum
        };
}
