using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.Common;
using FarmProject.Domain.Models;
using FarmProject.Presentation.Models.BreedingRabbits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages;

public class AllBreedingRabbitsModel(IBreedingRabbitService breedingRabbitService) : PageModel
{
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;

    public List<ViewBreedingRabbitDto>? BreedingRabbits { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await _breedingRabbitService.GetAllBreedingRabbits();

        return result.Match<IActionResult, List<BreedingRabbit>>(
            onSuccess: breedingRabbits =>
            {
                BreedingRabbits = breedingRabbits.Select(r => r.ToViewBreedingRabbitDto()).ToList();
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
