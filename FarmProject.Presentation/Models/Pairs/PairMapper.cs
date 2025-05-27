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
                MaleRabbit = pair.MaleRabbit.ToViewRabbitDto(),
                FemaleRabbit = pair.FemaleRabbit.ToViewRabbitDto(),
                StartDate = pair.StartDate,
                EndDate = pair.EndDate,
                PairingStatus = pair.PairingStatus
            };

    public static Result<Pair> ToPair(this ViewPairDto viewPairDto)
    {
        var maleRabbitResult = viewPairDto.MaleRabbit.ToRabbit();
        if (maleRabbitResult.IsFailure)
            return Result.Failure<Pair>(PairErrors.InvalidPairing);

        var femaleRabbitResult = viewPairDto.FemaleRabbit.ToRabbit();
        if (femaleRabbitResult.IsFailure)
            return Result.Failure<Pair>(PairErrors.InvalidPairing);

        var createdPair = new Pair(
                id: viewPairDto.Id,
                maleRabbit: maleRabbitResult.Value,
                femaleRabbit: femaleRabbitResult.Value,
                startDate: viewPairDto.StartDate
            );

        return Result.Success(createdPair);
    }
}
