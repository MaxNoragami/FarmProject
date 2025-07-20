using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.Sacrifications;

public class ViewSacrificationDto
{
    public int Id { get; set; }
    public SacrificationReason SacrificationReason { get; set; }
    public int CageId { get; set; }
    public OffspringType OffspringType { get; set; }
    public int Amount { get; set; }
    public DateTime BirthDate { get; set; }
    public int? OrderRequestId { get; set; } = null;
}
