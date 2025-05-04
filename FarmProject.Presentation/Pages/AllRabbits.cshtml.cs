using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Models;
using FarmProject.Presentation.Models.Rabbits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages
{
    public class AllRabbitsModel(IRabbitService rabbitService) : PageModel
    {
        private readonly IRabbitService _rabbitService = rabbitService;

        public List<ViewRabbitDto> Rabbits { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var requestedRabbits = await _rabbitService.GetAllRabbits();
                Rabbits = requestedRabbits.Select(r => r.ToViewRabbitDto()).ToList();
                return Page();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occurred when retrieving rabbits.");
            }
            
        }
    }
}
