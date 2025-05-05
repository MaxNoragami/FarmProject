using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.RabbitsService;
using FarmProject.Presentation.Models.Rabbits;
using FarmProject.Domain.Models;
using FarmProject.Application.Common;
using FarmProject.Domain.Errors;


namespace FarmProject.Presentation.Pages;

public class AddRabbitModel(IRabbitService rabbitService) : PageModel
{
    private readonly IRabbitService _rabbitService = rabbitService;

    [BindProperty]
    public CreateRabbitDto Rabbit { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _rabbitService.CreateRabbit(
                           Rabbit.Name, 
                           Rabbit.Gender, 
                           Rabbit.BreedingStatus);

        return result.Match<IActionResult, Rabbit>(
            onSuccess: rabbit =>
            {
                var createdRabbit = rabbit.ToViewRabbitDto();
                return RedirectToPage("/RabbitDetails", new { id = createdRabbit.Id });
            },

            onFailure: error =>
            {
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }
}
