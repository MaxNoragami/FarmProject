using FarmProject.Domain.Constants;

namespace FarmProject.Domain.Models;

public class Sacrification(
        int orderId,
        int breedingRabbitId,
        OffspringType type,
        int amount,
        DateTime birthDate) 
    : Entity
{
    public int OrderId { get; private set; } = orderId;
    public OffspringType Type { get; private set; } = type;
    public int Amount { get; private set; } = amount;
    public int BreedingRabbitId { get; private set; } = breedingRabbitId;
    public DateTime BirthDate { get; private set; } = birthDate;
}
