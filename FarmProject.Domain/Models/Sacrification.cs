using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;

namespace FarmProject.Domain.Models;

public class Sacrification : Entity
{
    public int? OrderRequestId { get; private set; }
    public SacrificationReason SacrificationReason { get; private set; }
    public Cage Cage { get; private set; }
    public OffspringType OffspringType { get; private set; }
    public int Amount { get; private set; }
    public DateTime BirthDate { get; private set; }

    private Sacrification() { }

    private Sacrification(
        Cage cage,
        int amount,
        DateTime birthDate,
        SacrificationReason sacrificationReason,
        int? orderRequestId = null)
    {
        OrderRequestId = orderRequestId;
        SacrificationReason = sacrificationReason;
        Cage = cage;
        OffspringType = cage.OffspringType;
        Amount = amount;
        BirthDate = birthDate;
    }

    public static Result<Sacrification> Create(
        Cage cage,
        int amount,
        SacrificationReason reason,
        OrderRequest? orderRequest = null)
    {
        var validationResult = ValidateCreation(cage, amount, reason, orderRequest);
        if (validationResult.IsFailure)
            return Result.Failure<Sacrification>(validationResult.Error);

        if (!cage.BirthDate.HasValue)
            return Result.Failure<Sacrification>(SacrificationErrors.NoBirthDate);

        var createdSacrification = new Sacrification(
            cage, amount, cage.BirthDate.Value, reason, orderRequest?.Id);

        if (reason == SacrificationReason.Order && orderRequest != null)
            createdSacrification.AddDomainEvent(new SacrificationCreatedEvent()
            {
                CageId = cage.Id,
                Amount = amount,
                OrderRequestId = orderRequest.Id
            });

        return Result.Success(createdSacrification);
    }

    private static Result ValidateCreation(
        Cage cage,
        int amount,
        SacrificationReason reason,
        OrderRequest? orderRequest)
    {
        if (reason == SacrificationReason.Order)
        {
            if (orderRequest == null)
                return Result.Failure(SacrificationErrors.OrderRequiresOrderRequest);

            return ValidateOrderRequest(cage, amount, orderRequest);
        }

        if (orderRequest != null)
            return Result.Failure(SacrificationErrors.OrderRequestNotAllowed);

        return Result.Success();
    }

    private static Result ValidateOrderRequest(Cage cage, int amount, OrderRequest orderRequest)
    {
        if (orderRequest.Cage.Id != cage.Id)
            return Result.Failure(SacrificationErrors.CageMismatch);

        if (amount > orderRequest.Amount)
            return Result.Failure(SacrificationErrors.ExceedsOrderAmount);

        return Result.Success();
    }
}
