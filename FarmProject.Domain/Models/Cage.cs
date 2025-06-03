using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;

namespace FarmProject.Domain.Models;

public class Cage(string name) : Entity
{
    public string Name { get; private set; } = name;
    public BreedingRabbit? MaleBreedingRabbit { get; private set; }
    public BreedingRabbit? FemaleBreedingRabbit { get; private set; }
    public int OffspringCount { get; private set; } = 0;
    public OffspringType OffspringType { get; private set; } = OffspringType.Mixed;

    public Result AssignBreedingRabbit(BreedingRabbit breedingRabbit)
    {
        if (breedingRabbit == null)
            return Result.Failure(CageErrors.InvalidAssignment);

        if (breedingRabbit.Gender == Gender.Female)
            FemaleBreedingRabbit = breedingRabbit;
        else
            MaleBreedingRabbit = breedingRabbit;

        breedingRabbit.CageId = Id;

        return Result.Success();
    }

    public Result<BreedingRabbit> RemoveBreedingRabbit(Gender gender)
    {
        BreedingRabbit? rabbitToRemove;

        if (gender == Gender.Female)
        {
            if (FemaleBreedingRabbit == null)
                return Result.Failure<BreedingRabbit>(CageErrors.NoFemaleRabbit);

            rabbitToRemove = FemaleBreedingRabbit;
            FemaleBreedingRabbit = null;
        }
        else
        {
            if (MaleBreedingRabbit == null)
                return Result.Failure<BreedingRabbit>(CageErrors.NoMaleRabbit);

            rabbitToRemove = MaleBreedingRabbit;
            MaleBreedingRabbit = null;
        }

        rabbitToRemove.CageId = null;

        return Result.Success(rabbitToRemove);
    }

    public Result AddOffspring(int count)
    {
        if (count < 0)
            return Result.Failure(CageErrors.NegativeOffspringNum);

        OffspringCount += count;
        return Result.Success();
    }

    public Result RemoveOffspring(int count)
    {
        if (count < 0)
            return Result.Failure(CageErrors.NegativeOffspringNum);
        if (count > OffspringCount)
            return Result.Failure(CageErrors.OverOffspringNum);

        OffspringCount -= count;
        return Result.Success();
    }

    public void SetOffspringType(OffspringType offspringType)
        => OffspringType = offspringType;
}
