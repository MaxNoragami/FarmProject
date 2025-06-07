using FarmProject.Domain.Constants;

namespace FarmProject.API.Dtos.BreedingRabbits;

public class UpdateBreedingRabbitDto
{
    public BreedingStatus? BreedingStatus { get; set; }
    public int? CageId { get; set; }
}
