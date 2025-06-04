using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Presentation.Models.BreedingRabbits;
using FarmProject.Domain.Models;
using FarmProject.Application.Common;
using FarmProject.Application.CageService;
using FarmProject.Presentation.Models.Cages;
using System.ComponentModel.DataAnnotations;

namespace FarmProject.Presentation.Pages;

public class AddBreedingRabbitModel(IBreedingRabbitService breedingRabbitService,
        ICageService cageService) 
    : PageModel
{
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;
    private readonly ICageService _cageService = cageService;


    [BindProperty]
    public CreateBreedingRabbitDto BreedingRabbit { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Cage ID is required")]
    public int CageId { get; set; }    

    public List<ViewCageDto> AvailableCages { get; private set; } = [];

    public async Task OnGet()
    {
        await LoadAvailableCages();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid)
        {
            await LoadAvailableCages();
            return Page();
        }

        var result = await _breedingRabbitService.CreateBreedingRabbitInCage(
                           BreedingRabbit.Name, 
                           BreedingRabbit.Gender,
                           CageId);

        if (result.IsSuccess)
        {
            var createdBreedingRabbit = result.Value.ToViewBreedingRabbitDto();
            return RedirectToPage("/BreedingRabbitDetails", new { id = createdBreedingRabbit.Id });
        }
        else
        {
            ModelState.AddModelError(string.Empty, result.Error.Description);
            await LoadAvailableCages();
            return Page();
        }
    }
    private async Task LoadAvailableCages()
    {
        var cagesResult = await _cageService.GetUnoccupiedCages();
        if (cagesResult.IsSuccess)
        {
            AvailableCages = cagesResult.Value
                .Select(c => c.ToViewCageDto())
                .ToList();
        }
    }
}
