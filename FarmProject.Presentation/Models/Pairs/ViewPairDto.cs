using FarmProject.Domain.Constants;
using FarmProject.Presentation.Models.BreedingRabbits;

namespace FarmProject.Presentation.Models.Pairs;

public class ViewPairDto
{
    public int Id { get; set; }
    public int MaleRabbitId { get; set; }
    public ViewBreedingRabbitDto? FemaleRabbit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public PairingStatus PairingStatus { get; set; }
}
