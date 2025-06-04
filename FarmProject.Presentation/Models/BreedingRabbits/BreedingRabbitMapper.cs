using FarmProject.Domain.Models;

namespace FarmProject.Presentation.Models.BreedingRabbits;

public static class BreedingRabbitMapper
{
    public static ViewBreedingRabbitDto ToViewBreedingRabbitDto(this BreedingRabbit breedingRabbit)
        => new ViewBreedingRabbitDto()
            {
                Id = breedingRabbit.Id,
                Name = breedingRabbit.Name,
                Gender = breedingRabbit.Gender,
                BreedingStatus = breedingRabbit.BreedingStatus,
                CageId = breedingRabbit.CageId
            };
}
