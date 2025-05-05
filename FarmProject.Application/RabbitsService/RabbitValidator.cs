using FarmProject.Domain.Constants;

namespace FarmProject.Application.RabbitsService;

public static class RabbitValidator
{
    public static void ValidateMakeBreedingStatus(Gender gender, BreedingStatus breedingStatus)
    {
        if (gender == Gender.Male &&
           breedingStatus != BreedingStatus.Available &&
           breedingStatus != BreedingStatus.Paired &&
           breedingStatus != BreedingStatus.Inapt)
        {
            throw new ArgumentException($"Status '{breedingStatus}' cannot be applied to a male.");
        }
    }
}
