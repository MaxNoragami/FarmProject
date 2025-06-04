using FarmProject.Application.CageService;
using FarmProject.Application.Common;
using FarmProject.Domain.Models;
using FarmProject.Presentation.Models.Cages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages;

public class AllCagesModel(ICageService cageService) : PageModel
{
    private readonly ICageService _cageService = cageService;

    public List<ViewCageDto>? Cages { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await _cageService.GetAllCages();

        return result.Match<IActionResult, List<Cage>>(
            onSuccess: cages =>
            {
                Cages = cages.Select(c => c.ToViewCageDto()).ToList();
                return Page();
            },

            onFailure: error =>
            {
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }
}
