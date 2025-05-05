using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;

namespace FarmProject.Application.RabbitsService;

public static class RabbitValidator
{
    public static Result ValidateMakeBreedingStatus(Gender gender, BreedingStatus breedingStatus)
    {
        if (gender == Gender.Male &&
           breedingStatus != BreedingStatus.Available &&
           breedingStatus != BreedingStatus.Paired &&
           breedingStatus != BreedingStatus.Inapt)
        {
            return Result.Failure(RabbitErrors.InvalidBreedingStatus);
        }

        return Result.Success();
    }
}
