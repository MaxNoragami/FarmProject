using System.ComponentModel.DataAnnotations;

namespace FarmProject.Presentation.Models.Cages;

public class CreateCageDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
    public string Name { get; set; }
}
