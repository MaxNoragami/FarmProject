using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.PairingService;
using FarmProject.Presentation.Models.Pairs;
using FarmProject.Domain.Models;
using FarmProject.Application.Common;


namespace FarmProject.Presentation.Pages;

public class AddPairModel(IPairingService pairingService) : PageModel
{
    private readonly IPairingService _pairingService = pairingService;

    [BindProperty]
    public CreatePairDto Pair { get; set; }

    public void OnGet()
    {
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
