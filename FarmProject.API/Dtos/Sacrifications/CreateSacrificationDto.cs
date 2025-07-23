using FarmProject.Domain.Constants;

namespace FarmProject.API.Dtos.Sacrifications;

public class CreateSacrificationDto
{
    public int CageId { get; set; }
    public int Amount { get; set; }
    public SacrificationReason SacrificationReason { get; set; }
    public int? OrderRequestId { get; set; } = null;
}
