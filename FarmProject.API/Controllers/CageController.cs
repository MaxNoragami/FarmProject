using FarmProject.Application.CageService;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using FarmProject.Application.Common;
using FarmProject.API.Dtos.Cages;

namespace FarmProject.API.Controllers;

[Route("api/cages")]
public class CageController(ICageService cageService) : AppBaseController
{
    private readonly ICageService _cageService = cageService;

    [HttpGet]
    public async Task<ActionResult<List<ViewCageDto>>> GetCages()
    {
        var result = await _cageService.GetAllCages();

        return result.Match<ActionResult, List<Cage>>(
            onSuccess: cages =>
            {
                var cagesView = cages.Select(c => c.ToViewCageDto()).ToList();
                return Ok(cagesView);
            },

            onFailure: error => 
                StatusCode(500, new { message = "An internal error occurred" })
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ViewCageDto>> GetCage(int id)
    {
        var result = await _cageService.GetCageById(id);

        return result.Match<ActionResult, Cage>(
            onSuccess: cage =>
            {
                var cageView = cage.ToViewCageDto();
                return Ok(cageView);
            },
            
            onFailure: error =>
                (error.Code == "Cage.NotFound")
                    ? NotFound(new { code = error.Code, message = error.Description })
                    : StatusCode(500, new { message = "An internal error occurred" })
        );
    }

    [HttpPost]
    public async Task<ActionResult<ViewCageDto>> CreateCage(CreateCageDto createCageDto)
    {
        var result = await _cageService.CreateCage(createCageDto.Name);

        return result.Match<ActionResult, Cage>(
            onSuccess: createdCage =>
            {
                var createdCageView = createdCage.ToViewCageDto();
                return Ok(createdCageView);
            },

            onFailure: error =>
                StatusCode(500, new { message = "An internal error occurred" })
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ViewCageDto>> UpdateCage(int id, UpdateCageDto updateCageDto)
    {
        var result = await _cageService.UpdateOffspringType(id, updateCageDto.OffspringType);

        return result.Match<ActionResult, Cage>(
            onSuccess: updatedCage =>
            {
                var cageView = updatedCage.ToViewCageDto();
                return Ok(cageView);
            },

            onFailure: error => 
                (error.Code == "Cage.NotFound")
                    ? NotFound(new { code = error.Code, message = error.Description })
                    : StatusCode(500, new { message = "An internal error occurred" })
        );

    }
}
