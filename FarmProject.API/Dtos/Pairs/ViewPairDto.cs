using FarmProject.Domain.Constants;

namespace FarmProject.API.Dtos.Pairs;

public class ViewPairDto
{
    public int Id { get; set; }
    public int FemaleRabbitId { get; set; }
    public int MaleRabbitId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public PairingStatus PairingStatus { get; set; }
}
