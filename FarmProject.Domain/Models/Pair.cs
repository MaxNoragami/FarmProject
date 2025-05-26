using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;

namespace FarmProject.Domain.Models;

public class Pair(int id, Rabbit maleRabbit, Rabbit femaleRabbit, DateTime startDate) : Entity(id)
{
    public Rabbit MaleRabbit => maleRabbit;
    public Rabbit FemaleRabbit => femaleRabbit;
    public DateTime StartDate { get; private set; } = startDate;
    public DateTime? EndDate { get; private set; } = null;
    public PairingStatus PairingStatus { get; set; } = PairingStatus.Active;

    public Result<FarmEvent> CreateNestPrepEvent(int eventId)
    {
        if (PairingStatus != PairingStatus.Successful)
            return Result.Failure<FarmEvent>(PairErrors.NotSuccessful);

        if (EndDate == null)
            return Result.Failure<FarmEvent>(PairErrors.NoEndDate);

        var dueDate = EndDate.Value.AddMonths(1).AddDays(-3);

        var message = $"Prepare nest in cage for rabbit #{FemaleRabbit.Id}";

        var nestPrepEvent = new FarmEvent(
            id: eventId,
            farmEventType: FarmEventType.NestPreparation,
            message: message,
            createdOn: EndDate.Value,
            dueOn: dueDate
        );

        return Result.Success(nestPrepEvent);
    }

    public Result RecordFailedImpregnation(DateTime dateTime)
    {
        if (PairingStatus != PairingStatus.Active)
            return Result.Failure(PairErrors.InvalidStateChange);

        MaleRabbit.SetBreedingStatus(BreedingStatus.Available);
        FemaleRabbit.SetBreedingStatus(BreedingStatus.Available);

        PairingStatus = PairingStatus.Failed;

        EndDate = dateTime;

        return Result.Success();
    }

    public Result RecordSuccessfulImpregnation(DateTime dateTime)
    {
        if (PairingStatus != PairingStatus.Active)
            return Result.Failure(PairErrors.InvalidStateChange);

        MaleRabbit.SetBreedingStatus(BreedingStatus.Available);
        FemaleRabbit.SetBreedingStatus(BreedingStatus.Pregnant);

        PairingStatus = PairingStatus.Successful;

        FarmEvents.Add(new FarmEvent(1, FarmEventType.NestPreparation, "", DateTime.Now, DateTime.Now));

        EndDate = dateTime;

        return Result.Success();
    }
}
