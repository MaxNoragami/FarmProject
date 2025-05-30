using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;

namespace FarmProject.Domain.Models;
public class Rabbit(string name, Gender gender) : Entity
{
    public string Name { get; private set; } = name;
    public Gender Gender { get; private set; } = gender;
    public BreedingStatus BreedingStatus { get; private set; } = BreedingStatus.Available;

    public Result<Pair> Breed(Rabbit otherRabbit, DateTime dateTimeNow)
    {
        var canPairResult = CanPairWith(otherRabbit);
        if (canPairResult.IsFailure)
            return Result.Failure<Pair>(canPairResult.Error);

        var setStatusResult = SetBreedingStatus(BreedingStatus.Paired);
        if (setStatusResult.IsFailure)
            return Result.Failure<Pair>(setStatusResult.Error);

        var otherSetStatusResult = otherRabbit.SetBreedingStatus(BreedingStatus.Paired);
        if (otherSetStatusResult.IsFailure)
            return Result.Failure<Pair>(otherSetStatusResult.Error);

        Events.Add(new BreedEvent()
            { 
                RabbitIds = [Id, otherRabbit.Id],
                StartDate = dateTimeNow
            }
        );

        var rabbitPair = new Pair(
            maleRabbit: (Gender == Gender.Male) ? this : otherRabbit,
            femaleRabbit: (Gender == Gender.Female) ? this : otherRabbit,
            startDate: dateTimeNow
        );

        return Result.Success(rabbitPair);
    }

    public Result CanPairWith(Rabbit otherRabbit)
    {
        if (Gender == otherRabbit.Gender)
            return Result.Failure(PairErrors.InvalidPairing);

        if(BreedingStatus != BreedingStatus.Available ||
           otherRabbit.BreedingStatus != BreedingStatus.Available)
        {
            return Result.Failure(PairErrors.InvalidPairing);
        }

        return Result.Success();
    }

    public Result SetBreedingStatus(BreedingStatus breedingStatus)
    {
        if (Gender == Gender.Male &&
           breedingStatus != BreedingStatus.Available &&
           breedingStatus != BreedingStatus.Paired &&
           breedingStatus != BreedingStatus.Inapt)
        {
            return Result.Failure(RabbitErrors.InvalidBreedingStatus);
        }

        BreedingStatus = breedingStatus;

        return Result.Success();
    }
}
