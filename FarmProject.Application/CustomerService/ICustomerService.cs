using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.CustomerService;

public interface ICustomerService
{
    public Task<Result<Customer>> AddCustomer(string firstName,
        string lastName,
        string email,
        string phoneNum);
    public Task<Result<PaginatedResult<Customer>>> GetPaginatedCustomers(
        PaginatedRequest<CustomerFilterDto> request);
    public Task<Result<Customer>> GetCustomerById(int customerId);
}
