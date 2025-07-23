using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.CustomerService;

public interface ICustomerRepository
{
    public Task<Customer> AddAsync(Customer customer);
    public Task<Customer?> GetByIdAsync(int customerId);
    public Task<PaginatedResult<Customer>> GetPaginatedAsync(PaginatedRequest<CustomerFilterDto> request);
    public Task<List<Customer>> FindAsync(ISpecification<Customer> specification);
    public Task<Customer> UpdateAsync(Customer customer);
    public Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken = default);
    public Task<bool> IsPhoneNumUsedAsync(string phoneNum, CancellationToken cancellationToken = default);
}
