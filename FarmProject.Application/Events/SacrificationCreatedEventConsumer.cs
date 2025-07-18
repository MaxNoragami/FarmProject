using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;

namespace FarmProject.Application.Events;

public class SacrificationCreatedEventConsumer(
        IUnitOfWork unitOfWork)
    : IEventConsumer<SacrificationCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> ConsumeAsync(SacrificationCreatedEvent domainEvent)
    {
        try
        {
            var cage = await _unitOfWork.CageRepository.GetByIdAsync(domainEvent.CageId);
            if (cage == null)
                return Result.Failure(CageErrors.NotFound);

            var orderRequest = await _unitOfWork.OrderRequestRepository
                .GetByIdAsync(domainEvent.OrderRequestId);
            if (orderRequest == null)
                return Result.Failure(OrderRequestErrors.NotFound);

            var processResult = orderRequest.ProcessSacrifice(domainEvent.Amount);
            if (processResult.IsFailure)
                return Result.Failure(processResult.Error);
            
            await _unitOfWork.OrderRequestRepository.UpdateAsync(orderRequest);
            await _unitOfWork.CageRepository.UpdateAsync(cage);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ConsumerErrors.ProcessingFailed);
        }
    }
}
