using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Domain.Models;
using FarmProject.Application.Common;
using FarmProject.Application.CageService;
using FarmProject.Presentation.Models.Cages;

namespace FarmProject.Presentation.Pages;

public class AddCageModel(ICageService cageService) : PageModel
{
    private readonly ICageService _cageService = cageService;

    [BindProperty]
    public CreateCageDto Cage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _cageService.CreateCage(
                           Cage.Name);

        return result.Match<IActionResult, Cage>(
            onSuccess: cage =>
            {
                var createdCage = cage.ToViewCageDto();
                return RedirectToPage("/CageDetails", new { id = createdCage.Id });
            },

            onFailure: error =>
            {
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }
}
