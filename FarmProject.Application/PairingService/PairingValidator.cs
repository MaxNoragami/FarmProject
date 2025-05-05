using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public static class PairingValidator
{
    public static bool ValidatePair(Rabbit firstAnimal, Rabbit secondAnimal)
    {
        if (firstAnimal.Gender != secondAnimal.Gender &&
            firstAnimal.BreedingStatus == BreedingStatus.Available &&
            secondAnimal.BreedingStatus == BreedingStatus.Available)
        {
            return true;
        }

        return false;
    }
}
