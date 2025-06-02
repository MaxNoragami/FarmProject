using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Presentation.Models.BreedingRabbits;
using FarmProject.Domain.Models;
using FarmProject.Application.Common;

namespace FarmProject.Presentation.Pages;

public class AddBreedingRabbitModel(IBreedingRabbitService breedingRabbitService) : PageModel
{
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;

    [BindProperty]
    public CreateBreedingRabbitDto BreedingRabbit { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _breedingRabbitService.CreateBreedingRabbit(
                           BreedingRabbit.Name, 
                           BreedingRabbit.Gender);

        return result.Match<IActionResult, BreedingRabbit>(
            onSuccess: breedingRabbit =>
            {
                var createdBreedingRabbit = breedingRabbit.ToViewBreedingRabbitDto();
                return RedirectToPage("/BreedingRabbitDetails", new { id = createdBreedingRabbit.Id });
            },

            onFailure: error =>
            {
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }
}
