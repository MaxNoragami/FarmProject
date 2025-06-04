using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Errors;
using FarmProject.Presentation.Models.BreedingRabbits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.CageService;
using FarmProject.Presentation.Models.Cages;

namespace FarmProject.Presentation.Pages;

public class BreedingRabbitDetailsModel(IBreedingRabbitService breedingRabbitService,
        ICageService cageService
    ) : PageModel
{
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;
    private readonly ICageService _cageService = cageService;

    [BindProperty]
    public UpdateBreedingRabbitDto UpdateBreedingRabbitDto { get; set; }

    [BindProperty]
    public int? DestinationCageId { get; set; }

    public ViewBreedingRabbitDto BreedingRabbitDto { get; private set; }
    public List<ViewCageDto> AvailableCages { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var result =  await _breedingRabbitService.GetBreedingRabbitById(id);

        if (result.IsSuccess)
        {
            BreedingRabbitDto = result.Value.ToViewBreedingRabbitDto();
            await LoadAvailableCages();
            return Page();
        }
        else
        {
            if (result.Error.Code == BreedingRabbitErrors.NotFound.Code)
                return NotFound();

            BreedingRabbitDto = new ViewBreedingRabbitDto() { Id = id };
            ModelState.AddModelError(string.Empty, result.Error.Description);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var breedingRabbitResult = await _breedingRabbitService.GetBreedingRabbitById(id);
        if (breedingRabbitResult.IsFailure)
        {
            if (breedingRabbitResult.Error.Code == BreedingRabbitErrors.NotFound.Code)
                return NotFound();

            ModelState.AddModelError(string.Empty, breedingRabbitResult.Error.Description);
            return Page();
        }

        BreedingRabbitDto = breedingRabbitResult.Value.ToViewBreedingRabbitDto();

        if (!ModelState.IsValid)
        {
            await LoadAvailableCages();
            return Page();
        }

        bool hasChanges = false;

        if (UpdateBreedingRabbitDto?.BreedingStatus != null &&
            UpdateBreedingRabbitDto.BreedingStatus != BreedingRabbitDto.BreedingStatus)
        {
            var statusResult = await _breedingRabbitService.UpdateBreedingStatus(id, UpdateBreedingRabbitDto.BreedingStatus.Value);

            if (statusResult.IsFailure)
            {
                ModelState.AddModelError(string.Empty, statusResult.Error.Description);
                await LoadAvailableCages();
                return Page();
            }

            BreedingRabbitDto = statusResult.Value.ToViewBreedingRabbitDto();
            hasChanges = true;
        }

        if (DestinationCageId.HasValue && DestinationCageId != BreedingRabbitDto.CageId)
        {
            var moveResult = await _cageService.MoveBreedingRabbitToCage(
                BreedingRabbitDto.Id, DestinationCageId.Value);

            if (moveResult.IsFailure)
            {
                ModelState.AddModelError(string.Empty, moveResult.Error.Description);
                await LoadAvailableCages();
                return Page();
            }

            var refreshResult = await _breedingRabbitService.GetBreedingRabbitById(id);
            if (refreshResult.IsSuccess)
                BreedingRabbitDto = refreshResult.Value.ToViewBreedingRabbitDto();

            hasChanges = true;
        }

        if (!hasChanges)
        {
            ModelState.AddModelError(string.Empty, "No changes were made. Please select a new breeding status or destination cage.");
            await LoadAvailableCages();
            return Page();
        }

        return RedirectToPage("./BreedingRabbitDetails", new { id = BreedingRabbitDto.Id });
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
