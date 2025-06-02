using FarmProject.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace FarmProject.Presentation.Models.BreedingRabbits;

public class ViewBreedingRabbitDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
    public string Name { get; set; }

    public Gender Gender { get; set; }

    public BreedingStatus BreedingStatus { get; set; }
}
