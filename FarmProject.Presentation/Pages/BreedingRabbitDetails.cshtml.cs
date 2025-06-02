using FarmProject.Application.Common;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FarmProject.Presentation.Models.BreedingRabbits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages;

public class BreedingRabbitDetailsModel(IBreedingRabbitService breedingRabbitService) : PageModel
{
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;

    [BindProperty]
    public UpdateBreedingRabbitDto UpdateBreedingRabbitDto { get; set; }
    public ViewBreedingRabbitDto BreedingRabbitDto { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var result =  await _breedingRabbitService.GetBreedingRabbitById(id);

        return result.Match<IActionResult, BreedingRabbit>(
            onSuccess: breedingRabbit =>
            {
                BreedingRabbitDto = breedingRabbit.ToViewBreedingRabbitDto();
                return Page();
            },
            
            onFailure: error =>
            {
                if (error.Code == BreedingRabbitErrors.NotFound.Code)
                    return NotFound();

                BreedingRabbitDto = new ViewBreedingRabbitDto() { Id = id };
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var breedingRabbitResult = await _breedingRabbitService.GetBreedingRabbitById(id);
        if (breedingRabbitResult.IsSuccess)
            BreedingRabbitDto = breedingRabbitResult.Value.ToViewBreedingRabbitDto();
        else
            BreedingRabbitDto = new ViewBreedingRabbitDto() { Id = id };

        if (!ModelState.IsValid)
            return Page();

        var result = await _breedingRabbitService.UpdateBreedingStatus(id, UpdateBreedingRabbitDto.BreedingStatus);

        return result.Match<IActionResult, BreedingRabbit>(
            onSuccess: updatedBreedingRabbit =>
            {
                BreedingRabbitDto = updatedBreedingRabbit.ToViewBreedingRabbitDto();
                return RedirectToPage("./BreedingRabbitDetails", new { id = BreedingRabbitDto.Id });
            },

            onFailure: error =>
            {
                if (error.Code == BreedingRabbitErrors.NotFound.Code)
                    return NotFound();

                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }
} 
