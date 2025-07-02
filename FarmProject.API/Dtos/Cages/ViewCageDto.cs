using FarmProject.Domain.Constants;

namespace FarmProject.API.Dtos.Cages;

public class ViewCageDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? BreedingRabbitId { get; set; }
    public int OffspringCount { get; set; } = 0;
    public OffspringType OffspringType { get; set; }
}
