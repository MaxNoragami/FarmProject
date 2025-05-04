using FarmProject.Application.Rabbits.Queries;
using FarmProject.Application.Rabbits.Responses;
using FarmProject.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmProject.Presentation.Pages
{
    public class AllRabbitsModel(IMediator mediator) : PageModel
    {
        private readonly IMediator _mediator = mediator;

        public List<RabbitDto> Rabbits { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var query = new GetAllRabbits();
                Rabbits = await _mediator.Send(query);
                return Page();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occurred when retrieving rabbits.");
            }
            
        }
    }
}
