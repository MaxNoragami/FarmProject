using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;
using FarmProject.Presentation.Models.Pairs;
using FarmProject.Presentation.Models.Rabbits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages;

public class AllPairsModel(IPairingService pairingService) : PageModel
{
    private readonly IPairingService _pairingService = pairingService;

    public List<ViewPairDto> PairDtos { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var requestedPairs = await _pairingService.GetAllPairs();
            PairDtos = requestedPairs.Select(p => p.ToViewPairDto()).ToList();
            return Page();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error occurred when retrieving pairs.");
        }
    }
}
