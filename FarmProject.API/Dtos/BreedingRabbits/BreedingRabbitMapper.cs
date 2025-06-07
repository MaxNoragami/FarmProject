using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.BreedingRabbits;

public static class BreedingRabbitMapper
{
    public static ViewBreedingRabbitDto ToViewBreedingRabbitDto(this BreedingRabbit breedingRabbit)
        => new ViewBreedingRabbitDto()
            {
                Id = breedingRabbit.Id,
                Name = breedingRabbit.Name,
                CageId = breedingRabbit.CageId,
                BreedingStatus = breedingRabbit.BreedingStatus
            };
}
