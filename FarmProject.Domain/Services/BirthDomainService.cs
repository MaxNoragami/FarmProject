using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;

namespace FarmProject.Domain.Services;

public class BirthDomainService
{
    public Result<BreedingRabbit> RecordBirth(
        BreedingRabbit breedingRabbit,
        Cage cage,
        int offspringCount,
        DateTime birthDate)
    {
        var birthResult = breedingRabbit.RecordBirth(offspringCount, DateTime.Now);
        if (birthResult.IsFailure)
            return Result.Failure<BreedingRabbit>(birthResult.Error);

        var offspringResult = cage.AddOffspring(offspringCount);
        if (offspringResult.IsFailure)
            return Result.Failure<BreedingRabbit>(offspringResult.Error);

        if (offspringCount > 0)
            cage.OffspringType = OffspringType.Mixed;

        return Result.Success(breedingRabbit);
    }

    public Result<OffspringSeparationEvent> WeanOffspring(
        Cage motherCage,
        Cage newCage,
        DateTime weanDate)
    {
        if (motherCage.BreedingRabbit == null || motherCage.BreedingRabbit.BreedingStatus != BreedingStatus.Nursing)
            return Result.Failure<OffspringSeparationEvent>(CageErrors.NoBreedingRabbit);

        if (newCage.BreedingRabbit != null || newCage.OffspringType != OffspringType.None)
            return Result.Failure<OffspringSeparationEvent>(CageErrors.Occupied);

        var offspringAmount = motherCage.OffspringCount;

        motherCage.RemoveOffspring(offspringAmount);
        motherCage.OffspringType = OffspringType.None;
        newCage.AddOffspring(offspringAmount);
        newCage.OffspringType = OffspringType.Mixed;
        motherCage.BreedingRabbit.BreedingStatus = BreedingStatus.Recovering;

        var offspringSeparationEvent = new OffspringSeparationEvent
        {
            NewCageId = newCage.Id,
            CreatedOn = weanDate
        };

        return Result.Success(offspringSeparationEvent);
    }

    public Result SeparateOffspring(
        Cage currentCage, 
        Cage? otherCage, 
        int? femaleOffspringCount)
    {
        var currentOffspringAmount = currentCage.OffspringCount;

        if (femaleOffspringCount.HasValue && femaleOffspringCount > currentOffspringAmount)
            return Result.Failure(CageErrors.InvalidSeparation);

        if (femaleOffspringCount.HasValue && femaleOffspringCount == currentOffspringAmount)
        {
            currentCage.OffspringType = OffspringType.Female;
            return Result.Success();
        }
        else if (!femaleOffspringCount.HasValue || femaleOffspringCount <= 0)
        {
            currentCage.OffspringType = OffspringType.Male;
            return Result.Success();
        }

        if (otherCage == null || otherCage.BreedingRabbit != null || otherCage.OffspringType != OffspringType.None)
            return Result.Failure(CageErrors.Occupied);

        currentCage.RemoveOffspring(femaleOffspringCount.Value);
        currentCage.OffspringType = OffspringType.Male;
        otherCage.AddOffspring(femaleOffspringCount.Value);
        otherCage.OffspringType = OffspringType.Female;

        return Result.Success();
    }
}
