using FarmProject.Application.FarmTaskService;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using FarmProject.Application.Common;
using FarmProject.Domain.Common;
using FarmProject.API.Dtos.FarmTasks;

namespace FarmProject.API.Controllers;

[Route("api/farm-tasks")]
public class FarmTaskController(IFarmTaskService farmTaskService) : AppBaseController
{
    private readonly IFarmTaskService _farmTaskService = farmTaskService;

    [HttpGet]
    public async Task<ActionResult<List<ViewFarmTaskDto>>> GetFarmTasksByDate([FromQuery] string? date)
    {
        var taskDate = ParseDate(date);
        var result = await _farmTaskService.GetAllFarmTasksByDate(taskDate);

        return result.Match<ActionResult, List<FarmTask>>(
                onSuccess: farmTasks =>
                {
                    var farmTasksView = farmTasks.Select(ft => ft.ToViewFarmTaskDto()).ToList();
                    return Ok(farmTasksView);
                },

                onFailure: error => HandleError(error)
            );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ViewFarmTaskDto>> MarkTaskCompleted(int id)
    {
        var result = await _farmTaskService.MarkFarmTaskAsCompleted(id);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewFarmTaskDto());
        else
            return HandleError(result.Error);
    }

    private static DateTime ParseDate(string? dateInput)
    {
        if (string.IsNullOrEmpty(dateInput))
            return DateTime.Today;

        if (DateTime.TryParse(dateInput, out var result))
            return result;

        return DateTime.Today;
    }

    private ActionResult HandleError(Error error)
    {
        switch (error.Code)
        {
            case string code when code.EndsWith(".NotFound"):
                return NotFound(new { code = error.Code, message = error.Description });

            case "FarmTask.NullValue":
            case "FarmTask.AlreadyCompleted":
                return BadRequest(new { code = error.Code, message = error.Description });

            default:
                return StatusCode(500, new { message = "An internal error occurred" });
        }
    }
}
