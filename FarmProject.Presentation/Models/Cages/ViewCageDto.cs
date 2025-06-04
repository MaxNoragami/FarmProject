using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace FarmProject.Presentation.Models.Cages;

public class ViewCageDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
    public string Name { get; set; }

    public BreedingRabbit? BreedingRabbit { get; set; }
    public int OffspringCount { get; set; } = 0;
    public OffspringType OffspringType { get; set; }
}
