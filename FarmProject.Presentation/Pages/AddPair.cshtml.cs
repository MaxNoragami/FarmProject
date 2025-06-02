using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.PairingService;
using FarmProject.Presentation.Models.Pairs;
using FarmProject.Domain.Models;
using FarmProject.Application.Common;
using FarmProject.Presentation.Models.BreedingRabbits;
using FarmProject.Application.BreedingRabbitsService;


namespace FarmProject.Presentation.Pages;

public class AddPairModel(IPairingService pairingService, IBreedingRabbitService breedingRabbitService) : PageModel
{
    private readonly IPairingService _pairingService = pairingService;
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;

    [BindProperty]
    public CreatePairDto Pair { get; set; }
    public List<ViewBreedingRabbitDto>? AllBreedingRabbitsDtos { get; private set; } = new();

    public async Task<IActionResult> OnGet()
    {
        var getAllBreedingRabbitsResult = await _breedingRabbitService.GetAllBreedingRabbits();
        return getAllBreedingRabbitsResult.Match<IActionResult, List<BreedingRabbit>>(
            onSuccess: breedingRabbits =>
            {
                AllBreedingRabbitsDtos = breedingRabbits.Select(r => r.ToViewBreedingRabbitDto()).ToList();
                return Page();
            },

            onFailure: error =>
            {
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var pairingResult = await _pairingService.CreatePair(
                           Pair.FemaleId,
                           Pair.MaleId);

        if (pairingResult.IsSuccess)
        {
            var createdPair = pairingResult.Value.ToViewPairDto();
            return RedirectToPage("/PairDetails", new { id = createdPair.Id });
        }
        else
        {
            ModelState.AddModelError(string.Empty, pairingResult.Error.Description);

            // Load breeding rabbits here
            var breedingRabbitsResult = await _breedingRabbitService.GetAllBreedingRabbits();
            if (breedingRabbitsResult.IsSuccess)
                AllBreedingRabbitsDtos = breedingRabbitsResult.Value.Select(r => r.ToViewBreedingRabbitDto()).ToList();

            return Page();
        }
    }
}
