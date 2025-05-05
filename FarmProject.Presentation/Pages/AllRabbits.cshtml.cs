using FarmProject.Application.RabbitsService;
using FarmProject.Application.Common;
using FarmProject.Domain.Models;
using FarmProject.Presentation.Models.Rabbits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages;

public class AllRabbitsModel(IRabbitService rabbitService) : PageModel
{
    private readonly IRabbitService _rabbitService = rabbitService;

    public List<ViewRabbitDto>? Rabbits { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await _rabbitService.GetAllRabbits();

        return result.Match<IActionResult, List<Rabbit>>(
            onSuccess: rabbits =>
            {
                Rabbits = rabbits.Select(r => r.ToViewRabbitDto()).ToList();
                return Page();
            },

            onFailure: error =>
            {
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }
}
