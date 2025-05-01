using FarmProject.Application.Rabbits.Queries;
using FarmProject.Application.Rabbits.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages
{
    public class RabbitDetailsModel(IMediator mediator) : PageModel
    {
        private readonly IMediator _mediator = mediator;

        public RabbitDto Rabbit { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var query = new GetRabbitById(id);
                Rabbit = await _mediator.Send(query);

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
