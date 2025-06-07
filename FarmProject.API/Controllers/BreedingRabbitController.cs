using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using FarmProject.Application.Common;
using FarmProject.API.Dtos.BreedingRabbits;
using FarmProject.Domain.Common;
using FarmProject.Application.CageService;
using FarmProject.Domain.Errors;


namespace FarmProject.API.Controllers;

[Route("api/breeding-rabbits")]
public class BreedingRabbitController(
        IBreedingRabbitService breedingRabbitService,
        ICageService cageService) 
    : AppBaseController
{
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;
    private readonly ICageService _cageService = cageService;

    [HttpGet]
    public async Task<ActionResult<List<ViewBreedingRabbitDto>>> GetBreedingRabbits(
        [FromQuery] bool availableForBreeding = false)
    {
        Result<List<BreedingRabbit>> result;

        if (availableForBreeding)
            result = await _breedingRabbitService.GetAllAvailableBreedingRabbits();
        else
            result = await _breedingRabbitService.GetAllBreedingRabbits();

        return result.Match<ActionResult, List<BreedingRabbit>>(
            onSuccess: breedingRabbits =>
            {
                var breedingRabbitsView = breedingRabbits.Select(br => br.ToViewBreedingRabbitDto()).ToList();
                return Ok(breedingRabbitsView);
            },

            onFailure: error => HandleError(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ViewBreedingRabbitDto>> GetBreedingRabbit(int id)
    {
        var result = await _breedingRabbitService.GetBreedingRabbitById(id);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewBreedingRabbitDto());
        else
            return HandleError(result.Error);
    }

    [HttpPost]
    public async Task<ActionResult<ViewBreedingRabbitDto>> CreateCage(CreateBreedingRabbitDto createBreedingRabbitDto)
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
            return HandleError(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ViewBreedingRabbitDto>> UpdateBreedingRabbit(
        int id, 
        UpdateBreedingRabbitDto? updateBreedingRabbitDto)
    {
        if (updateBreedingRabbitDto == null)
            return HandleError(BreedingRabbitErrors.NoChangesRequested);

        BreedingRabbit? updatedBreedingRabbit = null;

        if (updateBreedingRabbitDto.BreedingStatus.HasValue)
        {
            var breedingStatusResult = await _breedingRabbitService
                .UpdateBreedingStatus(id, updateBreedingRabbitDto.BreedingStatus.Value);

            if (breedingStatusResult.IsFailure)
                return HandleError(breedingStatusResult.Error);

            updatedBreedingRabbit = breedingStatusResult.Value;
        }

        if (updateBreedingRabbitDto.CageId.HasValue)
        {
            var moveBreedingRabbitResult = await _cageService
                .MoveBreedingRabbitToCage(id, updateBreedingRabbitDto.CageId.Value);

            if (moveBreedingRabbitResult.IsFailure)
                return HandleError(moveBreedingRabbitResult.Error);

            var getBreedingRabbitResult = await _breedingRabbitService
                .GetBreedingRabbitById(id);
            if (getBreedingRabbitResult.IsFailure)
                return HandleError(getBreedingRabbitResult.Error);

            updatedBreedingRabbit = getBreedingRabbitResult.Value;
        }

        if (updatedBreedingRabbit == null)
            return HandleError(BreedingRabbitErrors.NoChangesRequested);

        return Ok(updatedBreedingRabbit.ToViewBreedingRabbitDto());
    }

    private ActionResult HandleError(Error error)
    {
        switch (error.Code)
        {
            case string code when code.EndsWith(".NotFound"):
                return NotFound(new { code = error.Code, message = error.Description });

            case "Cage.InvalidAssignment":
            case "Cage.RabbitAlreadyInCage":
            case "BreedingRabbit.NoChangesRequested":
            case "Cage.Occupied":
                return BadRequest(new { code = error.Code, message = error.Description });

            case "BreedingRabbit.CreationFailed":
                return StatusCode(500, new { code = error.Code, message = error.Description });

            default:
                return StatusCode(500, new { message = "An internal error occurred" });
        }
    }
}