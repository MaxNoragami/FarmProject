using FarmProject.Domain.Models;
using FarmProject.Presentation.Models.BreedingRabbits;

namespace FarmProject.Presentation.Models.Pairs;

public static class PairMapper
{
    public static ViewPairDto ToViewPairDto(this Pair pair)
        => new ViewPairDto()
            {
                Id = pair.Id,
                MaleRabbitId = pair.MaleRabbitId,
                FemaleRabbit = pair.FemaleRabbit?.ToViewBreedingRabbitDto(),
                StartDate = pair.StartDate,
                EndDate = pair.EndDate,
                PairingStatus = pair.PairingStatus
            };
}
