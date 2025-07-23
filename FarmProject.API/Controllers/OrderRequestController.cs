using FarmProject.API.Attributes;
using FarmProject.API.Dtos.Orders;
using FarmProject.API.Dtos;
using FarmProject.Application.Common;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Application.IdentityService;
using FarmProject.Application.OrderRequestService;
using Microsoft.AspNetCore.Mvc;
using FarmProject.API.Dtos.OrderRequests;

namespace FarmProject.API.Controllers;

[Route("api/order-requests")]
[AuthorizeRoles(UserRole.Worker, UserRole.Logistics)]
public class OrderRequestController(
        IOrderRequestService orderRequestService) 
    : AppBaseController
{
    private readonly IOrderRequestService _orderRequestService = orderRequestService;

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ViewOrderRequestDto>>> GetPaginatedOrderRequests(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sort = "",
        [FromQuery] SortDirection defaultDirection = SortDirection.Ascending,
        [FromQuery] OrderRequestFilterDto? filter = null)
    {
        var request = new PaginatedRequest<OrderRequestFilterDto>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Filter = filter ?? new OrderRequestFilterDto(),
            Sort = new SortSpecification
            {
                Sort = sort,
                SortDirection = defaultDirection
            }
        };

        var result = await _orderRequestService.GetPaginatedOrderRequests(request);

        return result.Match(
            onSuccess: paginatedResult =>
            {
                var viewOrderRequestDtos = paginatedResult.Items.Select(or => or.ToViewOrderRequestDto()).ToList();

                var paginatedDtos = paginatedResult.ToPaginatedResult(viewOrderRequestDtos);

                return Ok(paginatedDtos);
            },
            onFailure: error => HandleError<PaginatedResult<ViewOrderRequestDto>>(error)
        );
    }
}
