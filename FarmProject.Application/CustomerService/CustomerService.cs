using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.CustomerService;

public class CustomerService(
        IUnitOfWork unitOfWork) 
    : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Customer>> AddCustomer(
        string firstName, 
        string lastName, 
        string email, 
        string phoneNum)
    {
        var createdCustomer = await _unitOfWork.CustomerRepository
            .AddAsync(
                new Customer(
                    firstName,
                    lastName,
                    email,
                    phoneNum
                )
            );
        return Result.Success(createdCustomer);
    }

    public async Task<Result<Customer>> GetCustomerById(int customerId)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
        if (customer == null)
            return Result.Failure<Customer>(CustomerErrors.NotFound);

        return Result.Success(customer);
    }

    public async Task<Result<PaginatedResult<Customer>>> GetPaginatedCustomers(
        PaginatedRequest<CustomerFilterDto> request)
    {
        var customers = await _unitOfWork.CustomerRepository.GetPaginatedAsync(request);

        return Result.Success(customers);
    }
}
