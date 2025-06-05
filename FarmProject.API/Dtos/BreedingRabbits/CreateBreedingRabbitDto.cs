using FarmProject.Domain.Constants;

namespace FarmProject.API.Dtos.BreedingRabbits;

public class CreateBreedingRabbitDto
{
    public string Name { get; set; }
    public int CageId { get; set; }
}
