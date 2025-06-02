using FarmProject.Application.FarmTaskService;
using FarmProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.Common;

namespace FarmProject.Presentation.Pages
{
    public class MarkCompletedModel(IFarmTaskService farmTaskService) : PageModel
    {
        private readonly IFarmTaskService _farmTaskService = farmTaskService;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReturnDate { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _farmTaskService.MarkFarmTaskAsCompleted(Id);

            return result.Match<IActionResult, FarmTask>(
                onSuccess: FarmTask =>
                {
                    TempData["SuccessMessage"] = "Task marked as completed successfully!";
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
