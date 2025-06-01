using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;

namespace FarmProject.Domain.Models;

public class Pair : Entity
{
    public Rabbit? MaleRabbit { get; private set; }
    public Rabbit? FemaleRabbit { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public PairingStatus PairingStatus { get; set; }

    public Pair(Rabbit maleRabbit, Rabbit femaleRabbit, DateTime startDate)
    {
        MaleRabbit = maleRabbit;
        FemaleRabbit = femaleRabbit;
        StartDate = startDate;
        EndDate = null;
        PairingStatus = PairingStatus.Active;
    }

    private Pair() { }

    public Result CreateNestPrepTask()
    {
        if (PairingStatus != PairingStatus.Successful)
            return Result.Failure<FarmTask>(PairErrors.NotSuccessful);

        if (EndDate == null)
            return Result.Failure<FarmTask>(PairErrors.NoEndDate);

        var dueDate = EndDate.Value.AddMonths(1).AddDays(-3);

        var message = $"Prepare nest in cage for rabbit #{FemaleRabbit!.Id}";

        AddDomainEvent(new NestPrepEvent()
        {
            DueDate = dueDate.Date,
            Message = message,
            CreatedOn = EndDate.Value
        });

        return Result.Success();
    }

    public Result RecordFailedImpregnation(DateTime dateTime)
    {
        if (PairingStatus != PairingStatus.Active)
            return Result.Failure(PairErrors.InvalidStateChange);

        MaleRabbit!.SetBreedingStatus(BreedingStatus.Available);
        FemaleRabbit!.SetBreedingStatus(BreedingStatus.Available);

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

        EndDate = dateTime;

        return Result.Success();
    }
}
