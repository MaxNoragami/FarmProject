using FarmProject.Application.Common;
using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
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
        var result =  await _rabbitService.GetRabbitById(id);

        return result.Match<IActionResult, Rabbit>(
            onSuccess: rabbit =>
            {
                RabbitDto = rabbit.ToViewRabbitDto();
                return Page();
            },
            
            onFailure: error =>
            {
                if (error.Code == RabbitErrors.NotFound.Code)
                    return NotFound();

                RabbitDto = new ViewRabbitDto() { Id = id };
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var rabbitResult = await _rabbitService.GetRabbitById(id);
        if (rabbitResult.IsSuccess)
            RabbitDto = rabbitResult.Value.ToViewRabbitDto();
        else
            RabbitDto = new ViewRabbitDto() { Id = id };

        if (!ModelState.IsValid)
            return Page();

        var result = await _rabbitService.UpdateBreedingStatus(id, UpdateRabbitDto.BreedingStatus);

        return result.Match<IActionResult, Rabbit>(
            onSuccess: updatedRabbit =>
            {
                RabbitDto = updatedRabbit.ToViewRabbitDto();
                return RedirectToPage("./RabbitDetails", new { id = RabbitDto.Id });
            },

            onFailure: error =>
            {
                if (error.Code == RabbitErrors.NotFound.Code)
                    return NotFound();

                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        );
    }
} 
