using FarmProject.Application.CageService;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using FarmProject.Application.Common;
using FarmProject.API.Dtos.Cages;
using FarmProject.Domain.Common;
using FarmProject.Application.Common.Models;
using FarmProject.API.Dtos;
using FarmProject.Application.Common.Models.Dtos;

namespace FarmProject.API.Controllers;

[Route("api/cages")]
public class CageController(ICageService cageService) : AppBaseController
{
    private readonly ICageService _cageService = cageService;

    [HttpGet]
    public async Task<ActionResult<List<ViewCageDto>>> GetCages(
        [FromQuery] bool unoccupiedCages = false)
    {
        Result<List<Cage>> result;

        if (unoccupiedCages)
            result = await _cageService.GetUnoccupiedCages();
        else
            result = await _cageService.GetAllCages();

        return result.Match<ActionResult<List<ViewCageDto>>, List<Cage>>(
            onSuccess: cages =>
            {
                var cagesView = cages.Select(c => c.ToViewCageDto()).ToList();
                return Ok(cagesView);
            },

            onFailure: error => HandleError<List<ViewCageDto>>(error)
        );
    }

    [HttpGet("paginated")]
    public async Task<ActionResult<PaginatedResult<ViewCageDto>>> GetPaginatedCages(
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] CageFilterDto? filter = null)
    {
        var request = new PaginatedRequest<CageFilterDto>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Filter = filter ?? new CageFilterDto()
        };

        var result = await _cageService.GetPaginatedCages(request);

        return result.Match(
            onSuccess: paginatedResult =>
            {
                var viewCageDtos = paginatedResult.Items.Select(c => c.ToViewCageDto()).ToList();

                var paginatedDtos = paginatedResult.ToPaginatedResult(viewCageDtos);

                return Ok(paginatedDtos);
            },
            onFailure: error => HandleError<PaginatedResult<ViewCageDto>>(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ViewCageDto>> GetCage(int id)
    {
        var result = await _cageService.GetCageById(id);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewCageDto());
        else
            return HandleError<ViewCageDto>(result.Error);
    }

    [HttpPost]
    public async Task<ActionResult<ViewCageDto>> CreateCage(CreateCageDto createCageDto)
    {
        var result = await _cageService.CreateCage(createCageDto.Name);

        if (result.IsSuccess)
        {
            var createdCage = result.Value.ToViewCageDto();
            return CreatedAtAction(nameof(GetCage), new { id = createdCage.Id }, createdCage);
        }
        else
            return HandleError<ViewCageDto>(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ViewCageDto>> UpdateCage(int id, UpdateCageDto updateCageDto)
    {
        var result = await _cageService.UpdateOffspringType(id, updateCageDto.OffspringType);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewCageDto());
        else
            return HandleError<ViewCageDto>(result.Error);
    }
}
