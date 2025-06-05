using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using FarmProject.Application.Common;
using FarmProject.API.Dtos.BreedingRabbits;


namespace FarmProject.API.Controllers;

[Route("api/breeding-rabbits")]
public class BreedingRabbitController(
        IBreedingRabbitService breedingRabbitService) 
    : AppBaseController
{
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService; 

    [HttpGet]
    public async Task<ActionResult<List<ViewBreedingRabbitDto>>> GetBreedingRabbits()
    {
        var result = await _breedingRabbitService.GetAllBreedingRabbits();

        return result.Match<ActionResult, List<BreedingRabbit>>(
            onSuccess: breedingRabbits =>
            {
                var breedingRabbitsView = breedingRabbits.Select(br => br.ToViewBreedingRabbitDto()).ToList();
                return Ok(breedingRabbitsView);
            },

            onFailure: error =>
                StatusCode(500, new { message = "An internal error occurred" })
        );
    }

    [HttpPost]
    public async Task<ActionResult<ViewBreedingRabbitDto>> CreateCage(CreateBreedingRabbitDto createBreedingRabbitDto)
    {
        var result = await _breedingRabbitService
            .AddBreedingRabbitToFarm(createBreedingRabbitDto.Name, 
                createBreedingRabbitDto.CageId);

        return result.Match<ActionResult, BreedingRabbit>(
            onSuccess: createdBreedingRabbit =>
            {
                var createdBreedingRabbitView = createdBreedingRabbit.ToViewBreedingRabbitDto();
                return Ok(createdBreedingRabbitView);
            },

            onFailure: error =>
            {
                switch (error.Code)
                {
                    case string code when code.EndsWith(".NotFound"):
                        return NotFound(new { code = error.Code, message = error.Description });

                    case "Cage.InvalidAssignment":
                    case "Cage.RabbitAlreadyInCage":
                    case "Cage.Occupied":
                        return BadRequest(new { code = error.Code, message = error.Description });

                    case "BreedingRabbit.CreationFailed":
                        return StatusCode(500, new { code = error.Code, message = error.Description });

                    default:
                        return StatusCode(500, new { message = "An internal error occurred" });
                }
            }
        );
    }

}