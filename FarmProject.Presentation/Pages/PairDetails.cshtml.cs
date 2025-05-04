using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;
using FarmProject.Presentation.Models.Pairs;
using FarmProject.Presentation.Models.Rabbits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace FarmProject.Presentation.Pages;

public class PairDetailsModel(IPairingService pairingService) : PageModel
{
    private readonly IPairingService _pairingService = pairingService;

    [BindProperty]
    public UpdatePairDto UpdatePairDto { get; set; }
    public ViewPairDto PairDto { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var requestedPair = await _pairingService.GetPairById(id);
            PairDto = requestedPair.ToViewPairDto();

            return Page();
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error occurred when trying to retrieve a pair.");
        }

    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var updatedPair = await _pairingService.UpdatePairingStatus(id, UpdatePairDto.PairingStatus);
            PairDto = updatedPair.ToViewPairDto();
            
            return RedirectToPage("./PairDetails", new { id = PairDto.Id });
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Update failed: " + ex.Message);
            return Page();
        }
    }
}
