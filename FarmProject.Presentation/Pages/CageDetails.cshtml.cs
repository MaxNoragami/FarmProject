using FarmProject.Application.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.CageService;
using FarmProject.Presentation.Models.Cages;

namespace FarmProject.Presentation.Pages;

public class CageDetailsModel(ICageService cageService) : PageModel
{
    private readonly ICageService _cageService = cageService;

    [BindProperty]
    public UpdateCageDto UpdateCageDto { get; set; }
    public ViewCageDto CageDto { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var result =  await _cageService.GetCageById(id);

        return result.Match<IActionResult, Cage>(
            onSuccess: cage =>
            {
                CageDto = cage.ToViewCageDto();
                return Page();
            },
            
            onFailure: error =>
            {
                if (error.Code == CageErrors.NotFound.Code)
                    return NotFound();

                CageDto = new ViewCageDto() { Id = id };
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var cageResult = await _cageService.GetCageById(id);
        if (cageResult.IsSuccess)
            CageDto = cageResult.Value.ToViewCageDto();
        else
            CageDto = new ViewCageDto() { Id = id };

        if (!ModelState.IsValid)
            return Page();

        var result = await _cageService.UpdateOffspringType(id, UpdateCageDto.OffspringType);

        return result.Match<IActionResult, Cage>(
            onSuccess: updatedCage =>
            {
                CageDto = updatedCage.ToViewCageDto();
                return RedirectToPage("./CageDetails", new { id = updatedCage.Id });
            },

            onFailure: error =>
            {
                if (error.Code == CageErrors.NotFound.Code)
                    return NotFound();

                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }
} 
