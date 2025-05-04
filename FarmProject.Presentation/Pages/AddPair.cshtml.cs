using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.PairingService;
using FarmProject.Presentation.Models.Pairs;


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

        var createdPair = await _pairingService.CreatePair(
                           Pair.FemaleId,
                           Pair.MaleId);

        var result = createdPair.ToViewPairDto();

        return RedirectToPage("/PairDetails", new { id = result.Id });
    }
}
