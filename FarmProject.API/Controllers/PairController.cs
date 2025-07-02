using FarmProject.API.Attributes;
using FarmProject.API.Dtos;
using FarmProject.API.Dtos.Pairs;
using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.IdentityService;
using FarmProject.Application.PairingService;
using Microsoft.AspNetCore.Mvc;

namespace FarmProject.API.Controllers;

[Route("api/pairs")]
[AuthorizeRoles(UserRole.Worker, UserRole.Logistics)]
public class PairController(IPairingService pairingService) : AppBaseController
{
    private readonly IPairingService _pairingService = pairingService;

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ViewPairDto>>> GetPaginatedPairs(
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string sort = "",
    [FromQuery] SortDirection defaultDirection = SortDirection.Ascending,
    [FromQuery] PairFilterDto? filter = null)
    {
        var request = new PaginatedRequest<PairFilterDto>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Filter = filter ?? new PairFilterDto(),
            Sort = new SortSpecification
            {
                Sort = sort,
                SortDirection = defaultDirection
            }
        };

        var result = await _pairingService.GetPaginatedPairs(request);

        return result.Match(
            onSuccess: paginatedResult =>
            {
                var viewPairsDtos = paginatedResult.Items.Select(p => p.ToViewPairDto()).ToList();

                var paginatedDtos = paginatedResult.ToPaginatedResult(viewPairsDtos);

                return Ok(paginatedDtos);
            },
            onFailure: error => HandleError<PaginatedResult<ViewPairDto>>(error)
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
