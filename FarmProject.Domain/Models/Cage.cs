using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;

namespace FarmProject.Domain.Models;

public class Cage(string name) : Entity
{
    public string Name { get; private set; } = name;
    public BreedingRabbit? BreedingRabbit { get; private set; }
    public int OffspringCount { get; private set; } = 0;
    public int ReservedOffspringCount { get; private set; } = 0;
    public OffspringType OffspringType { get; set; } = OffspringType.None;
    public DateTime? BirthDate { get; private set; }
    public bool IsSacrificable { get; set; } = false;

    public Result AssignBreedingRabbit(BreedingRabbit breedingRabbit)
    {
        if (breedingRabbit == null)
            return Result.Failure(CageErrors.InvalidAssignment);

        if (breedingRabbit.CageId != null)
            return Result.Failure(CageErrors.RabbitAlreadyInCage);

        if (BreedingRabbit != null)
                return Result.Failure(CageErrors.Occupied);

        BreedingRabbit = breedingRabbit;
        breedingRabbit.CageId = Id;

        return Result.Success();
    }

    public Result<BreedingRabbit> RemoveBreedingRabbit()
    {
        BreedingRabbit? rabbitToRemove;

        if (BreedingRabbit == null)
            return Result.Failure<BreedingRabbit>(CageErrors.NoBreedingRabbit);

        rabbitToRemove = BreedingRabbit;
        BreedingRabbit = null;
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

    public void RecordBirthDate(DateTime birthDate)
    {
        BirthDate = birthDate;
        IsSacrificable = false;
    }

    public void ResetBirthDate()
    {
        if (OffspringCount <= 0)
        {
            BirthDate = null;
            IsSacrificable = false;
        }
    }

    public void UpdateSacrificableStatus(int sacrificableAgeInDays)
    {
        if (OffspringCount > 0 && BirthDate.HasValue)
        {
            var ageInDays = (DateTime.UtcNow - BirthDate.Value).TotalDays;
            IsSacrificable = ageInDays >= sacrificableAgeInDays;
        }
    }

    public Result SacrificeOffspring(int count)
    {
        if (count <= 0)
            return Result.Failure(CageErrors.InvalidSacrificeCount);
        if (count > OffspringCount)
            return Result.Failure(CageErrors.OverOffspringNum);
        if (!IsSacrificable)
            return Result.Failure(CageErrors.NotSacrificable);

        OffspringCount -= count;

        if (OffspringCount == 0)
        {
            BirthDate = null;
            OffspringType = OffspringType.None;
            IsSacrificable = false;
        }

        return Result.Success();
    }
}
