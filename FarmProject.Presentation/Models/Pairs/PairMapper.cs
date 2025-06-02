using FarmProject.Domain.Models;
using FarmProject.Presentation.Models.BreedingRabbits;

namespace FarmProject.Presentation.Models.Pairs;

public static class PairMapper
{
    public static ViewPairDto ToViewPairDto(this Pair pair)
        => new ViewPairDto()
            {
                Id = pair.Id,
                MaleBreedingRabbit = pair.MaleBreedingRabbit?.ToViewBreedingRabbitDto(),
                FemaleBreedingRabbit = pair.FemaleBreedingRabbit?.ToViewBreedingRabbitDto(),
                StartDate = pair.StartDate,
                EndDate = pair.EndDate,
                PairingStatus = pair.PairingStatus
            };
}
