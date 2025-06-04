using FarmProject.Domain.Models;

namespace FarmProject.Presentation.Models.Cages;

public static class CageMapper
{
    public static ViewCageDto ToViewCageDto(this Cage cage)
        => new ViewCageDto()
            {
                Id = cage.Id,
                Name = cage.Name,
                BreedingRabbit = cage.BreedingRabbit,
                OffspringCount = cage.OffspringCount,
                OffspringType = cage.OffspringType
            };
}
