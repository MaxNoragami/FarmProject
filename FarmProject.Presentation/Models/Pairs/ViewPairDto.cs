using FarmProject.Domain.Constants;
using FarmProject.Presentation.Models.Rabbits;

namespace FarmProject.Presentation.Models.Pairs;

public class ViewPairDto
{
    public int Id { get; set; }
    public ViewRabbitDto MaleRabbit { get; set; }
    public ViewRabbitDto FemaleRabbit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public PairingStatus PairingStatus { get; set; }
}
