using FarmProject.Application.PairingService;
using FarmProject.Application.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FarmProject.Presentation.Models.Pairs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Presentation.Models.Rabbits;
namespace FarmProject.Presentation.Pages;

public class PairDetailsModel(IPairingService pairingService) : PageModel
{
    private readonly IPairingService _pairingService = pairingService;

    [BindProperty]
    public UpdatePairDto UpdatePairDto { get; set; }
    public ViewPairDto PairDto { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var result = await _pairingService.GetPairById(id); 

        return result.Match<IActionResult, Pair>(
            onSuccess: pair =>
            {
                PairDto = pair.ToViewPairDto();
                return Page();
            },

            onFailure: error =>
            {
                if (error.Code == PairErrors.NotFound.Code)
                    return NotFound();

                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var pairResult = await _pairingService.GetPairById(id);
        if (pairResult.IsSuccess)
            PairDto = pairResult.Value.ToViewPairDto();
        else
            PairDto = new ViewPairDto() { Id = id };

        if (!ModelState.IsValid)
            return Page();

        var result = await _pairingService.UpdatePairingStatus(id, UpdatePairDto.PairingStatus);

        return result.Match<IActionResult, Pair>(
            onSuccess: updatedPair =>
            {
                PairDto = updatedPair.ToViewPairDto();
                return RedirectToPage("./PairDetails", new { id = PairDto.Id });
            },

            onFailure: error =>
            {
                if (error.Code == PairErrors.NotFound.Code)
                    return NotFound();

                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }
}
