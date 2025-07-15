using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.Cages;

public static class CageMapper
{
    public static ViewCageDto ToViewCageDto(this Cage cage)
        => new ViewCageDto()
            {
                Id = cage.Id,
                Name = cage.Name,
                BreedingRabbitId = cage.BreedingRabbit?.Id,
                OffspringCount = cage.OffspringCount,
                ReservedOffspringCount = cage.ReservedOffspringCount,
                OffspringType = cage.OffspringType,
                BirthDate = cage.BirthDate,
                IsSacrificable = cage.IsSacrificable
            };
}
