using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Events;

namespace FarmProject.Domain.Models;
public class BreedingRabbit(string name) : Entity
{
    public string Name { get; private set; } = name;
    public BreedingStatus BreedingStatus { get; private set; } = BreedingStatus.Available;
    public int? CageId { get; set; }

    public Result Breed(int maleId, DateTime dateTimeNow)
    {
        SetBreedingStatus(BreedingStatus.Paired);

        AddDomainEvent(new BreedEvent()
            { 
                BreedingRabbitIds = [Id, maleId],
                StartDate = dateTimeNow
            }
        );

        return Result.Success();
    }

    public void SetBreedingStatus(BreedingStatus breedingStatus)
        => BreedingStatus = breedingStatus;
}
