using FarmProject.Application.CageService;
using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Validators;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;
using System.Xml.Linq;

namespace FarmProject.Application.CustomerService;

public class ValidationCustomerService(
        ICustomerService inner,
        ValidationHelper validationHelper) 
    : ICustomerService
{
    private readonly ICustomerService _inner = inner;
    private readonly ValidationHelper _validationHelper = validationHelper;

    public Task<Result<Customer>> AddCustomer(
        string firstName, 
        string lastName, 
        string email, 
        string phoneNum
    )
        => _validationHelper.ValidateAndExecute(
                new AddCustomerParam(firstName, lastName, email, phoneNum),
                () => _inner.AddCustomer(firstName, lastName, email, phoneNum));

    public Task<Result<Customer>> GetCustomerById(int customerId)
        => _inner.GetCustomerById(customerId);

    public Task<Result<PaginatedResult<Customer>>> GetPaginatedCustomers(
        PaginatedRequest<CustomerFilterDto> request
    )
        => _validationHelper.ValidateAndExecute(
                new PaginatedRequestParam<CustomerFilterDto>(request),
                () => _inner.GetPaginatedCustomers(request));
}

public record AddCustomerParam(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNum
    );