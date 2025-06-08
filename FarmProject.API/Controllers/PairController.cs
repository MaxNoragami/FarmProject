using FarmProject.Application.PairingService;
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

        return result.Match<ActionResult<List<ViewPairDto>>, List<Pair>>(
            onSuccess: pairs =>
            {
                var pairsView = pairs.Select(p => p.ToViewPairDto()).ToList();
                return Ok(pairsView);
            },

            onFailure: error => HandleError<List<ViewPairDto>>(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ViewPairDto>> GetPair(int id)
    {
        var result = await _pairingService.GetPairById(id);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewPairDto());
        else
            return HandleError<ViewPairDto>(result.Error);
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
            return HandleError<ViewPairDto>(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ViewPairDto>> UpdatePair(int id, UpdatePairDto updatePairDto)
    {
        var result = await _pairingService.UpdatePairingStatus(id, updatePairDto.PairingStatus);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewPairDto());
        else
            return HandleError<ViewPairDto>(result.Error);
    }
}
