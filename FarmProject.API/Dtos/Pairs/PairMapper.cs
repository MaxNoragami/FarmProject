using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.Pairs;

public static class PairMapper
{
    public static ViewPairDto ToViewPairDto(this Pair pair)
        => new ViewPairDto()
        {
            Id = pair.Id,
            FemaleRabbitId = pair.FemaleRabbit.Id,
            MaleRabbitId = pair.MaleRabbitId,
            StartDate = pair.StartDate,
            EndDate = pair.EndDate,
            PairingStatus = pair.PairingStatus
        };
}
