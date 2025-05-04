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
}
