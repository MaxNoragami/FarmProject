using FarmProject.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace FarmProject.Presentation.Models.BreedingRabbits;

public class CreateBreedingRabbitDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
    public string Name { get; set; }
    public BreedingStatus BreedingStatus { get; set; }
    [Required(ErrorMessage = "Cage selection is mandatory")]
    public int CageId { get; set; }
}
