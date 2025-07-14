using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models.SortConfigs;
using FarmProject.Application.CustomerService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class CustomerRepository(
        FarmDbContext context) 
    : ICustomerRepository
{
    private readonly FarmDbContext _context = context;

    public async Task<Customer> AddAsync(Customer customer)
    {
        _context.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<List<Customer>> FindAsync(ISpecification<Customer> specification)
        => await _context.Customers
            .Where(specification.ToExpression())
            .ToListAsync();

    public async Task<Customer?> GetByIdAsync(int customerId)
        => await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == customerId);

    public async Task<PaginatedResult<Customer>> GetPaginatedAsync(PaginatedRequest<CustomerFilterDto> request)
    {
        var query = _context.Customers
            .AsQueryable();

        if (request.Filter != null)
            query = query.ApplyFilter(request.Filter);

        var sortOrders = request.Sort?.ToSortOrders(CustomerSortingFields.AllowedSortFields)
            ?? new List<SortOrder> { new SortOrder { PropertyName = "Id", Direction = SortDirection.Ascending } };
        query = query.ApplySorting(sortOrders, CustomerSortingFields.PropertyPaths);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var result = new PaginatedResult<Customer>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return result;
    }

    public async Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Customers
            .AnyAsync(c => c.Email == email, cancellationToken);

    public async Task<bool> IsPhoneNumUsedAsync(string phoneNum, CancellationToken cancellationToken = default)
        => await _context.Customers
            .AnyAsync(c => c.PhoneNum == phoneNum, cancellationToken);

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
}
