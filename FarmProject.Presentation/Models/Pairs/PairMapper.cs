using FarmProject.Domain.Models;
using FarmProject.Domain.Constants;
using FarmProject.Presentation.Models.Rabbits;
using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;

namespace FarmProject.Presentation.Models.Pairs;

public static class PairMapper
{
    public static ViewPairDto ToViewPairDto(this Pair pair)
        => new ViewPairDto()
            {
                Id = pair.Id,
                MaleRabbit = pair.MaleRabbit?.ToViewRabbitDto(),
                FemaleRabbit = pair.FemaleRabbit?.ToViewRabbitDto(),
                StartDate = pair.StartDate,
                EndDate = pair.EndDate,
                PairingStatus = pair.PairingStatus
            };
}
