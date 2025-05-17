using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.PairingService;
using FarmProject.Presentation.Models.Pairs;
using FarmProject.Domain.Models;
using FarmProject.Application.Common;
using FarmProject.Presentation.Models.Rabbits;
using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Common;


namespace FarmProject.Presentation.Pages;

public class AddPairModel(IPairingService pairingService, IRabbitService rabbitService) : PageModel
{
    private readonly IPairingService _pairingService = pairingService;
    private readonly IRabbitService _rabbitService = rabbitService;

    [BindProperty]
    public CreatePairDto Pair { get; set; }
    public List<ViewRabbitDto>? AllRabbitsDtos { get; private set; }

    public async Task<IActionResult> OnGet()
    {
        var getAllRabbitsResult = await _rabbitService.GetAllRabbits();
        return getAllRabbitsResult.Match<IActionResult, List<Rabbit>>(
            onSuccess: rabbits =>
            {
                AllRabbitsDtos = rabbits.Select(r => r.ToViewRabbitDto()).ToList();
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
        if(!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _pairingService.CreatePair(
                           Pair.FemaleId,
                           Pair.MaleId);

        return result.Match<IActionResult, Pair>(
            onSuccess: pair =>
            {
                var createdPair = pair.ToViewPairDto();
                return RedirectToPage("/PairDetails", new { id = createdPair.Id });
            },

            onFailure: error =>
            {
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }
}
