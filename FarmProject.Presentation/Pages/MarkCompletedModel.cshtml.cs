using FarmProject.Application.EventsService;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.Common;

namespace FarmProject.Presentation.Pages
{
    public class MarkCompletedModel(IFarmEventService farmEventService) : PageModel
    {
        private readonly IFarmEventService _farmEventService = farmEventService;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReturnDate { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _farmEventService.MarkFarmEventAsCompleted(Id);

            return result.Match<IActionResult, FarmEvent>(
                onSuccess: FarmEvent =>
                {
                    TempData["SuccessMessage"] = "Event marked as completed successfully!";
                    return RedirectToPage("/Index", new { dateInput = ReturnDate });
                },

                onFailure: error =>
                {
                    TempData["ErrorMessage"] = error.Description;
                    return RedirectToPage("/Index", new { dateInput = ReturnDate });
                }
            );
        }
    }
}
