using FarmProject.Application.FarmTaskService;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using FarmProject.Application.Common;
using FarmProject.API.Dtos;
using FarmProject.API.Dtos.FarmTasks;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;

namespace FarmProject.API.Controllers;

[Route("api/farm-tasks")]
public class FarmTaskController(IFarmTaskService farmTaskService) : AppBaseController
{
    private readonly IFarmTaskService _farmTaskService = farmTaskService;

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ViewFarmTaskDto>>> GetPaginatedFarmTasks(
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string sort = "",
    [FromQuery] SortDirection defaultDirection = SortDirection.Ascending,
    [FromQuery] FarmTaskFilterDto? filter = null)
    {
        var request = new PaginatedRequest<FarmTaskFilterDto>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Filter = filter ?? new FarmTaskFilterDto(),
            Sort = new SortSpecification
            {
                Sort = sort,
                SortDirection = defaultDirection
            }
        };

        var result = await _farmTaskService.GetPaginatedFarmTasks(request);

        return result.Match(
            onSuccess: paginatedResult =>
            {
                var viewFarmTaskDtos = paginatedResult.Items.Select(ft => ft.ToViewFarmTaskDto()).ToList();

                var paginatedDtos = paginatedResult.ToPaginatedResult(viewFarmTaskDtos);

                return Ok(paginatedDtos);
            },
            onFailure: error => HandleError<PaginatedResult<ViewFarmTaskDto>>(error)
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ViewFarmTaskDto>> MarkTaskCompleted(int id)
    {
        var result = await _farmTaskService.MarkFarmTaskAsCompleted(id);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewFarmTaskDto());
        else
            return HandleError<ViewFarmTaskDto>(result.Error);
    }

    private static DateTime ParseDate(string? dateInput)
    {
        if (string.IsNullOrEmpty(dateInput))
            return DateTime.Today;

        if (DateTime.TryParse(dateInput, out var result))
            return result;

        return DateTime.Today;
    }
}
