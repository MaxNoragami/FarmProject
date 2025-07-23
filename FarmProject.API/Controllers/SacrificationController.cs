using FarmProject.API.Attributes;
using FarmProject.API.Dtos;
using FarmProject.API.Dtos.Sacrifications;
using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.IdentityService;
using FarmProject.Application.SacrificationService;
using Microsoft.AspNetCore.Mvc;

namespace FarmProject.API.Controllers;

[Route("api/sacrifications")]
[AuthorizeRoles(UserRole.Worker, UserRole.Logistics)]
public class SacrificationController(
        ISacrificationService sacrificationService) 
    : AppBaseController
{
    private readonly ISacrificationService _sacrificationService = sacrificationService;

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ViewSacrificationDto>>> GetPaginatedSacrifications(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sort = "",
        [FromQuery] SortDirection defaultDirection = SortDirection.Ascending,
        [FromQuery] SacrificationFilterDto? filter = null)
    {
        var request = new PaginatedRequest<SacrificationFilterDto>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Filter = filter ?? new SacrificationFilterDto(),
            Sort = new SortSpecification
            {
                Sort = sort,
                SortDirection = defaultDirection
            }
        };

        var result = await _sacrificationService.GetPaginatedSacrifications(request);

        return result.Match(
            onSuccess: paginatedResult =>
            {
                var viewSacrificationDtos = paginatedResult.Items
                    .Select(s => s.ToViewSacrificationDto()).ToList();
                var paginatedDtos = paginatedResult.ToPaginatedResult(viewSacrificationDtos);
                return Ok(paginatedDtos);
            },
            onFailure: error => HandleError<PaginatedResult<ViewSacrificationDto>>(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ViewSacrificationDto>> GetSacrification(int id)
    {
        var result = await _sacrificationService.GetSacrificationById(id);

        if (result.IsSuccess)
            return Ok(result.Value.ToViewSacrificationDto());
        else
            return HandleError<ViewSacrificationDto>(result.Error);
    }

    [HttpPost]
    public async Task<ActionResult<ViewSacrificationDto>> CreateSacrification(
        CreateSacrificationDto createSacrificationDto)
    {
        var result = await _sacrificationService.SacrificeOffspring(
            createSacrificationDto.CageId,
            createSacrificationDto.Amount,
            createSacrificationDto.SacrificationReason,
            createSacrificationDto.OrderRequestId);

        if (result.IsSuccess)
        {
            var createdSacrification = result.Value.ToViewSacrificationDto();
            return CreatedAtAction(nameof(GetSacrification),
                new { id = createdSacrification.Id }, createdSacrification);
        }
        else
            return HandleError<ViewSacrificationDto>(result.Error);
    }
}
