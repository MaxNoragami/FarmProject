using FarmProject.Application.RabbitsService;
using FarmProject.Presentation.Models.Rabbits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages
{
    public class RabbitDetailsModel(IRabbitService rabbitService) : PageModel
    {
        private readonly IRabbitService _rabbitService = rabbitService;

        public ViewRabbitDto Rabbit { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var requestedRabbit =  await _rabbitService.GetRabbitById(id);
                Rabbit = requestedRabbit.ToViewRabbitDto();
                
                return Page();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occurred when trying to retrieve a rabbit.");
            }
            
        }
    } 
}
