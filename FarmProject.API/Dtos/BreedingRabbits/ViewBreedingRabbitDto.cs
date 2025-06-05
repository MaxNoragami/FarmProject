using FarmProject.Domain.Constants;

namespace FarmProject.API.Dtos.BreedingRabbits;

public class ViewBreedingRabbitDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? CageId { get; set; }
    public BreedingStatus BreedingStatus { get; set; }
}
