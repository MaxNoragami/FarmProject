using FarmProject.Application.RabbitsService;
using FarmProject.Presentation.Models.Pairs;
using FarmProject.Presentation.Models.Rabbits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages;

public class RabbitDetailsModel(IRabbitService rabbitService) : PageModel
{
    private readonly IRabbitService _rabbitService = rabbitService;

    [BindProperty]
    public UpdateRabbitDto UpdateRabbitDto { get; set; }
    public ViewRabbitDto RabbitDto { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var requestedRabbit =  await _rabbitService.GetRabbitById(id);
            RabbitDto = requestedRabbit.ToViewRabbitDto();
            
            return Page();
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error occurred when trying to retrieve a rabbit.");
        }   
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var updatedRabbit = await _rabbitService.UpdateBreedingStatus(id, UpdateRabbitDto.BreedingStatus);
            RabbitDto = updatedRabbit.ToViewRabbitDto();

            return RedirectToPage("./RabbitDetails", new { id = RabbitDto.Id });
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
