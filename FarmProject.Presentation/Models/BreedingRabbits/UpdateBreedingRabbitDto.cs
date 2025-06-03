using FarmProject.Domain.Constants;

namespace FarmProject.Presentation.Models.BreedingRabbits;

public class UpdateBreedingRabbitDto
{
    public BreedingStatus BreedingStatus { get; set; }
    public int? CageId { get; set; }
}
