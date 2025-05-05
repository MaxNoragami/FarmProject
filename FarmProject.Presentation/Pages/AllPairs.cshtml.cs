using FarmProject.Application.PairingService;
using FarmProject.Application.Common;
using FarmProject.Domain.Models;
using FarmProject.Presentation.Models.Pairs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages;

public class AllPairsModel(IPairingService pairingService) : PageModel
{
    private readonly IPairingService _pairingService = pairingService;

    public List<ViewPairDto>? PairDtos { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await _pairingService.GetAllPairs();

        return result.Match<IActionResult, List<Pair>>(
            onSuccess: pairs =>
            {
                PairDtos = pairs.Select(p => p.ToViewPairDto()).ToList();
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
