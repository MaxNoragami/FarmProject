using FarmProject.Domain.Constants;

namespace FarmProject.Domain.Models;

public class Sacrification(
        Cage cage,
        int amount,
        DateTime birthDate,
        SacrificationReason sacrificationReason,
        int? orderRequestId = null) 
    : Entity
{
    public int? OrderRequestId { get; private set; } = orderRequestId;
    public SacrificationReason SacrificationReason { get; private set; } = sacrificationReason;
    public Cage Cage { get; private set; } = cage;
    public OffspringType OffspringType { get; private set; } = cage.OffspringType;
    public int Amount { get; private set; } = amount;
    public DateTime BirthDate { get; private set; } = birthDate;
}
