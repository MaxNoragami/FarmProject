using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FarmProject.Application.RabbitsService;
using FarmProject.Presentation.Models.Rabbits;


namespace FarmProject.Presentation.Pages
{
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

            var createdRabbit = await _rabbitService.CreateRabbit(
                               Rabbit.Name, 
                               Rabbit.Gender, 
                               Rabbit.BreedingStatus);

            var result = createdRabbit.ToViewRabbitDto();

            return RedirectToPage("/RabbitDetails", new { id = result.Id });
        }
    }
}
