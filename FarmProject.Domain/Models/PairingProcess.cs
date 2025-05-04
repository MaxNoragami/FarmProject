using FarmProject.Domain.Constants;

namespace FarmProject.Domain.Models;

public class PairingProcess
{
    public int Id { get; set; }
    public int MaleId { get; set; }
    public int FemaleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public PairingStatus PairingStatus { get; set; }
}
