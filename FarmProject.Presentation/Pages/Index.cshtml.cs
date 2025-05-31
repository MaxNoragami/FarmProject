using FarmProject.Application.Common;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;
using FarmProject.Presentation.Models.FarmTasks;
using Microsoft.AspNetCore.Mvc;
using FarmProject.Application.FarmTaskService;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages
{
    public class IndexModel(ILogger<IndexModel> logger, IFarmTaskService farmTaskService) : PageModel
    {
        private readonly ILogger<IndexModel> _logger = logger;
        private readonly IFarmTaskService _farmTaskService = farmTaskService;

        public List<ViewFarmTaskDto>? FarmTasks { get; private set; }

        [BindProperty(SupportsGet = true)]
        public string? DateInput { get; set; }

        public DateTime CurrentDate { get; private set; }

        public string NextDateString =>
        CurrentDate < DateTime.MaxValue.AddDays(-1)
            ? CurrentDate.AddDays(1).ToString("yyyy-MM-dd")
            : CurrentDate.ToString("yyyy-MM-dd");
        
        public string PrevDateString =>
            CurrentDate > DateTime.MinValue.AddDays(1)
                ? CurrentDate.AddDays(-1).ToString("yyyy-MM-dd")
                : CurrentDate.ToString("yyyy-MM-dd");


        public async Task<IActionResult> OnGetAsync()
        {
            CurrentDate = ParseDate(DateInput);

            var result = await _farmTaskService.GetAllFarmTasksByDate(CurrentDate);

            return result.Match<IActionResult, List<FarmTask>>(
                onSuccess: farmTasks =>
                {
                    FarmTasks = farmTasks.Select(farmTask => farmTask.ToViewFarmTaskDto()).ToList();
                    return Page();
                },

                onFailure: error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }
            );
        }

        private DateTime ParseDate(string? dateInput)
        {
            if (string.IsNullOrEmpty(dateInput))
                return DateTime.Today;

            if (DateTime.TryParse(dateInput, out var result))
                return result;

            return DateTime.Today;
        }
    }
}
