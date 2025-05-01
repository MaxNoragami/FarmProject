using FarmProject.Application.Rabbits.Create;
using FarmProject.Application.Rabbits.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages
{
    public class AddRabbitModel(IMediator mediator) : PageModel
    {
        private readonly IMediator _mediator = mediator;

        [BindProperty]
        public RabbitDto Rabbit { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var command = new CreateRabbit(
                               Rabbit.Name, 
                               Rabbit.Gender, 
                               Rabbit.Breedable);

            var result = await _mediator.Send(command);

            return RedirectToPage("/RabbitDetails", new { id = result.Id });
        }
    }
}
