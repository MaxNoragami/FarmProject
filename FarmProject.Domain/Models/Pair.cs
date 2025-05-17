using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;

namespace FarmProject.Domain.Models;

public class Pair(int id, int maleId, int femaleId, DateTime startDate) : Entity(id)
{
    public int MaleId { get; private set; } = maleId;
    public int FemaleId { get; private set; } = femaleId;
    public DateTime StartDate { get; private set; } = startDate;
    public DateTime? EndDate { get; private set; } = null;
    public PairingStatus PairingStatus { get; private set; } = PairingStatus.Active;

    public Result CompletePairing(PairingStatus outcome, Rabbit maleRabbit, Rabbit femaleRabbit, DateTime dateTimeNow)
    {
        if (PairingStatus != PairingStatus.Active)
            return Result.Failure(PairErrors.InvalidStateChange);

        if (outcome == PairingStatus.Active)
            return Result.Failure(PairErrors.InvalidOutcome);

        EndDate = dateTimeNow;

        Result femaleResult;
        if (outcome == PairingStatus.Successful)
            femaleResult = femaleRabbit.SetBreedingStatus(BreedingStatus.Pregnant);
        else
            femaleResult = femaleRabbit.SetBreedingStatus(BreedingStatus.Available);

        if (femaleResult.IsFailure)
            return Result.Failure(femaleResult.Error);

        var maleResult = maleRabbit.SetBreedingStatus(BreedingStatus.Available);
        if (maleResult.IsFailure)
            return Result.Failure(maleResult.Error);
        
        PairingStatus = outcome;
        return Result.Success();
    }
}
