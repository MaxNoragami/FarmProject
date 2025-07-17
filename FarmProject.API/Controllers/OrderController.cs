using FarmProject.API.Attributes;
using FarmProject.API.Dtos;
using FarmProject.API.Dtos.Orders;
using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.IdentityService;
using FarmProject.Application.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace FarmProject.API.Controllers;

[Route("api/orders")]
[AuthorizeRoles(UserRole.Worker, UserRole.Logistics)]
public class OrderController(
        IOrderService orderService)
    : AppBaseController
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ViewOrdersDto>>> GetPaginatedOrders(
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string sort = "",
    [FromQuery] SortDirection defaultDirection = SortDirection.Ascending,
    [FromQuery] OrderFilterDto? filter = null)
    {
        var request = new PaginatedRequest<OrderFilterDto>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Filter = filter ?? new OrderFilterDto(),
            Sort = new SortSpecification
            {
                Sort = sort,
                SortDirection = defaultDirection
            }
        };

        var result = await _orderService.GetPaginatedOrders(request);

        return result.Match(
            onSuccess: paginatedResult =>
            {
                var viewOrderDtos = paginatedResult.Items.Select(o => o.ToViewOrdersDto()).ToList();

                var paginatedDtos = paginatedResult.ToPaginatedResult(viewOrderDtos);

                return Ok(paginatedDtos);
            },
            onFailure: error => HandleError<PaginatedResult<ViewOrdersDto>>(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ViewSingleOrderDto>> GetOrder(int id)
    {
        var result = await _orderService.GetOrderById(id);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewSingleOrderDto());
        else
            return HandleError<ViewSingleOrderDto>(result.Error);
    }

    [HttpPost]
    public async Task<ActionResult<ViewSingleOrderDto>> CreateOrder(CreateOrderDto createOrderDto)
    {
        var result = await _orderService.CreateOrder(
            createOrderDto.CustomerId,
            createOrderDto.OrderRequests);

        if (result.IsSuccess)
        {
            var createdOrder = result.Value.ToViewSingleOrderDto();
            return CreatedAtAction(nameof(createdOrder), new { id = createdOrder.Id }, createdOrder);
        }
        else
            return HandleError<ViewSingleOrderDto>(result.Error);
    }
}
