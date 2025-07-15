using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.CustomerService;

public class LoggingCustomerService(
        ICustomerService customerService,
        LoggingHelper loggingHelper)
    : ICustomerService
{
    private readonly ICustomerService _customerService = customerService;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<Customer>> AddCustomer(
        string firstName, 
        string lastName, 
        string email, 
        string phoneNum
    )
        => await _loggingHelper.LogOperation(
                LoggingUtilities.FormatMethodCall(
                    nameof(AddCustomer),
                    (nameof(firstName), firstName),
                    (nameof(lastName), lastName),
                    (nameof(email), email),
                    (nameof(phoneNum), phoneNum)
                ),
                async () =>
                    await _customerService.AddCustomer(
                        firstName, lastName, email, phoneNum));

    public async Task<Result<Customer>> GetCustomerById(int customerId)
        => await _loggingHelper.LogOperation(
                LoggingUtilities.FormatMethodCall(
                    nameof(GetCustomerById),
                    (nameof(customerId), customerId)
                ),
                async () =>
                    await _customerService.GetCustomerById(customerId));

    public async Task<Result<PaginatedResult<Customer>>> GetPaginatedCustomers(
        PaginatedRequest<CustomerFilterDto> request
    )
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetPaginatedCustomers), 
                (nameof(request), request)
            ),
            async () =>
                await _customerService.GetPaginatedCustomers(request));
}
