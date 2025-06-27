using FarmProject.API.Attributes;
using FarmProject.API.Dtos;
using FarmProject.API.Dtos.BreedingRabbits;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.CageService;
using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Identity;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace FarmProject.API.Controllers;

[Route("api/breeding-rabbits")]
[AuthorizeRoles(UserRole.Worker, UserRole.Logistics)]
public class BreedingRabbitController(
        IBreedingRabbitService breedingRabbitService,
        ICageService cageService) 
    : AppBaseController
{
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;
    private readonly ICageService _cageService = cageService;

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ViewBreedingRabbitDto>>> GetPaginatedBreedingRabbits(
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string sort = "",
    [FromQuery] SortDirection defaultDirection = SortDirection.Ascending,
    [FromQuery] BreedingRabbitFilterDto? filter = null)
    {
        var request = new PaginatedRequest<BreedingRabbitFilterDto>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Filter = filter ?? new BreedingRabbitFilterDto(),
            Sort = new SortSpecification
            {
                Sort = sort,
                SortDirection = defaultDirection
            }
        };

        var result = await _breedingRabbitService.GetPaginatedBreedingRabbits(request);

        return result.Match(
            onSuccess: paginatedResult =>
            {
                var viewBreedingRabbitsDtos = paginatedResult.Items.Select(br => br.ToViewBreedingRabbitDto()).ToList();

                var paginatedDtos = paginatedResult.ToPaginatedResult(viewBreedingRabbitsDtos);

                return Ok(paginatedDtos);
            },
            onFailure: error => HandleError<PaginatedResult<ViewBreedingRabbitDto>>(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ViewBreedingRabbitDto>> GetBreedingRabbit(int id)
    {
        var result = await _breedingRabbitService.GetBreedingRabbitById(id);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewBreedingRabbitDto());
        else
            return HandleError<ViewBreedingRabbitDto>(result.Error);
    }

    [HttpPost]
    public async Task<ActionResult<ViewBreedingRabbitDto>> CreateBreedingRabbit(CreateBreedingRabbitDto createBreedingRabbitDto)
    {
        var result = await _breedingRabbitService
            .AddBreedingRabbitToFarm(createBreedingRabbitDto.Name, 
                createBreedingRabbitDto.CageId);

        if (result.IsSuccess)
        {
            var createdBreedingRabbit = result.Value.ToViewBreedingRabbitDto();
            return CreatedAtAction(nameof(GetBreedingRabbit), 
                new { id = createdBreedingRabbit.Id }, createdBreedingRabbit);
        }
        else
            return HandleError<ViewBreedingRabbitDto>(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ViewBreedingRabbitDto>> UpdateBreedingRabbit(
        int id, 
        UpdateBreedingRabbitDto? updateBreedingRabbitDto)
    {
        if (updateBreedingRabbitDto == null)
            return HandleError<ViewBreedingRabbitDto>(BreedingRabbitErrors.NoChangesRequested);

        BreedingRabbit? updatedBreedingRabbit = null;

        if (updateBreedingRabbitDto.BreedingStatus.HasValue)
        {
            var breedingStatusResult = await _breedingRabbitService
                .UpdateBreedingStatus(id, updateBreedingRabbitDto.BreedingStatus.Value);

            if (breedingStatusResult.IsFailure)
                return HandleError<ViewBreedingRabbitDto>(breedingStatusResult.Error);

            updatedBreedingRabbit = breedingStatusResult.Value;
        }

        if (updateBreedingRabbitDto.CageId.HasValue)
        {
            var moveBreedingRabbitResult = await _cageService
                .MoveBreedingRabbitToCage(id, updateBreedingRabbitDto.CageId.Value);

            if (moveBreedingRabbitResult.IsFailure)
                return HandleError<ViewBreedingRabbitDto>(moveBreedingRabbitResult.Error);

            var getBreedingRabbitResult = await _breedingRabbitService
                .GetBreedingRabbitById(id);
            if (getBreedingRabbitResult.IsFailure)
                return HandleError<ViewBreedingRabbitDto>(getBreedingRabbitResult.Error);

            updatedBreedingRabbit = getBreedingRabbitResult.Value;
        }

        if (updatedBreedingRabbit == null)
            return HandleError<ViewBreedingRabbitDto>(BreedingRabbitErrors.NoChangesRequested);

        return Ok(updatedBreedingRabbit.ToViewBreedingRabbitDto());
    }
}