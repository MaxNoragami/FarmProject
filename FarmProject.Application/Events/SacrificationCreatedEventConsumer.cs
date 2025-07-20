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
            if (!domainEvent.OrderRequestId.HasValue)
                return Result.Success();

            var cage = await _unitOfWork.CageRepository.GetByIdAsync(domainEvent.CageId);
            if (cage == null)
                return Result.Failure(CageErrors.NotFound);

            var orderRequest = await _unitOfWork.OrderRequestRepository
                .GetByIdAsync(domainEvent.OrderRequestId.Value);
            if (orderRequest == null)
                return Result.Failure(OrderRequestErrors.NotFound);

            var processResult = orderRequest.ProcessSacrifice(domainEvent.Amount);
            if (processResult.IsFailure)
                return Result.Failure(processResult.Error);
            
            await _unitOfWork.OrderRequestRepository.UpdateAsync(orderRequest);
            await _unitOfWork.CageRepository.UpdateAsync(cage);

            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderRequest.OrderId);
            if (order != null)
            {
                var updateOrderResult = order.UpdateStatusBasedOnOrderRequests();
                if (updateOrderResult.IsSuccess)
                    await _unitOfWork.OrderRepository.UpdateAsync(order);
            }

            return Result.Success();
        }
        catch
        {
            return Result.Failure(ConsumerErrors.ProcessingFailed);
        }
    }
}
