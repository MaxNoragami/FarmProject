using FarmProject.Application.PairingService;
using FarmProject.Domain.Common;
using FarmProject.Application.Common;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using FarmProject.API.Dtos.Pairs;

namespace FarmProject.API.Controllers;

[Route("api/pairs")]
public class PairController(IPairingService pairingService) : AppBaseController
{
    private readonly IPairingService _pairingService = pairingService;

    [HttpGet]
    public async Task<ActionResult<List<ViewPairDto>>> GetPairs()
    {
        var result = await _pairingService.GetAllPairs();

        return result.Match<ActionResult, List<Pair>>(
            onSuccess: pairs =>
            {
                var pairsView = pairs.Select(p => p.ToViewPairDto()).ToList();
                return Ok(pairsView);
            },

            onFailure: error => HandleError(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ViewPairDto>> GetPair(int id)
    {
        var result = await _pairingService.GetPairById(id);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewPairDto());
        else
            return HandleError(result.Error);
    }

    [HttpPost]
    public async Task<ActionResult<ViewPairDto>> CreatePair(CreatePairDto createPairDto)
    {
        var result = await _pairingService
            .CreatePair(createPairDto.FemaleRabbitId, createPairDto.MaleRabbitId);

        if (result.IsSuccess)
        {
            var createdPair = result.Value.ToViewPairDto();
            return CreatedAtAction(nameof(GetPair), new { id = createdPair.Id }, createdPair);
        }
        else
            return HandleError(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ViewPairDto>> UpdatePair(int id, UpdatePairDto updatePairDto)
    {
        var result = await _pairingService.UpdatePairingStatus(id, updatePairDto.PairingStatus);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewPairDto());
        else
            return HandleError(result.Error);
    }

    private ActionResult HandleError(Error error)
    {
        switch (error.Code)
        {
            case string code when code.EndsWith(".NotFound"):
                return NotFound(new { code = error.Code, message = error.Description });

            case "BreedingRabbit.NotAvailableToPair":
            case "Pair.InvalidStateChange":
            case "Pair.NotSuccessful":
            case "Pair.NoEndDate":
            case "Pair.InvalidOutcome":
                return BadRequest(new { code = error.Code, message = error.Description });

            case "Pair.CreationFailed":
            case "Pair.UpdateFailed":
                return StatusCode(500, new { code = error.Code, message = error.Description });

            default:
                return StatusCode(500, new { message = "An internal error occurred" });
        }
    }
}
