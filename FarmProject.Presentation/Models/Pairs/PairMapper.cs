using FarmProject.Domain.Models;
using FarmProject.Domain.Constants;

namespace FarmProject.Presentation.Models.Pairs;

public static class PairMapper
{
    public static ViewPairDto ToViewPairDto(this Pair pair)
        => new ViewPairDto()
            {
                Id = pair.Id,
                MaleId = pair.MaleId,
                FemaleId = pair.FemaleId,
                StartDate = pair.StartDate,
                EndDate = pair.EndDate,
                PairingStatus = pair.PairingStatus
            };
    
    public static Pair ToPair(this ViewPairDto viewPairDto)
        => new Pair(
                id: viewPairDto.Id,
                maleId: viewPairDto.MaleId,
                femaleId: viewPairDto.FemaleId,
                startDate: viewPairDto.StartDate
            );
}
