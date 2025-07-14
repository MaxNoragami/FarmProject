using FarmProject.API.Attributes;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Application.CustomerService;
using FarmProject.Application.IdentityService;
using Microsoft.AspNetCore.Mvc;
using FarmProject.API.Dtos;
using FarmProject.API.Dtos.Customers;
using FarmProject.Application.Common;
using FarmProject.API.Dtos.Cages;
using FarmProject.Domain.Models;

namespace FarmProject.API.Controllers;

[Route("api/customers")]
[AuthorizeRoles(UserRole.Worker, UserRole.Logistics)]
public class CustomerController(
        ICustomerService customerService) 
    : AppBaseController
{
    private readonly ICustomerService _customerService = customerService;

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ViewCustomerDto>>> GetPaginatedCustomers(
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string sort = "",
    [FromQuery] SortDirection defaultDirection = SortDirection.Ascending,
    [FromQuery] CustomerFilterDto? filter = null)
    {
        var request = new PaginatedRequest<CustomerFilterDto>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Filter = filter ?? new CustomerFilterDto(),
            Sort = new SortSpecification
            {
                Sort = sort,
                SortDirection = defaultDirection
            }
        };

        var result = await _customerService.GetPaginatedCustomers(request);

        return result.Match(
            onSuccess: paginatedResult =>
            {
                var viewCustomerDtos = paginatedResult.Items.Select(c => c.ToViewCustomerDto()).ToList();

                var paginatedDtos = paginatedResult.ToPaginatedResult(viewCustomerDtos);

                return Ok(paginatedDtos);
            },
            onFailure: error => HandleError<PaginatedResult<ViewCustomerDto>>(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ViewCustomerDto>> GetCustomer(int id)
    {
        var result = await _customerService.GetCustomerById(id);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewCustomerDto());
        else
            return HandleError<ViewCustomerDto>(result.Error);
    }

    [HttpPost]
    public async Task<ActionResult<ViewCustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto)
    {
        var result = await _customerService.AddCustomer(
            createCustomerDto.FirstName,
            createCustomerDto.LastName,
            createCustomerDto.Email,
            createCustomerDto.PhoneNum);

        if (result.IsSuccess)
        {
            var createdCustomer = result.Value.ToViewCustomerDto();
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
        }
        else
            return HandleError<ViewCustomerDto>(result.Error);
    }
}
