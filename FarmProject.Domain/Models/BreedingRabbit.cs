﻿using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;

namespace FarmProject.Domain.Models;

public class BreedingRabbit(string name) : Entity
{
    public string Name { get; private set; } = name;
    
    public BreedingStatus BreedingStatus { get; set; } = BreedingStatus.Available;

    public int? CageId { get; set; }

    public Result Breed(int maleId, DateTime dateTimeNow)
    {
        if (BreedingStatus != BreedingStatus.Available)
            return Result.Failure(BreedingRabbitErrors.NotAvailableToPair);
            
        BreedingStatus = BreedingStatus.Paired;

        AddDomainEvent(new BreedEvent()
            { 
                BreedingRabbitIds = [Id, maleId],
                StartDate = dateTimeNow
            }
        );

        return Result.Success();
    }

    public Result RecordBirth(int offspringCount, DateTime birthDate)
    {
        if (BreedingStatus != BreedingStatus.Pregnant)
            return Result.Failure(BreedingRabbitErrors.NotPregnant);

        if (offspringCount == 0)
            BreedingStatus = BreedingStatus.Recovering;
        else
            BreedingStatus = BreedingStatus.Nursing;

        AddDomainEvent(new BirthEvent
            {
                BreedingRabbitId = Id,
                CageId = CageId!.Value,
                OffspringCount = offspringCount,
                BirthDate = birthDate
            }
        );

        return Result.Success();
    }
}
