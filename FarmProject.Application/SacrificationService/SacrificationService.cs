using FarmProject.Application.CageService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Events;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.SacrificationService;

public class SacrificationService(
        IUnitOfWork unitOfWork,
        DomainEventDispatcher domainEventDispatcher) 
    : ISacrificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly DomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;

    public async Task<Result<Sacrification>> SacrificeOffspring(
        int cageId, int amount, SacrificationReason sacrificationReason, int? orderRequestId)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(cageId);
        if (cage == null)
            return Result.Failure<Sacrification>(CageErrors.NotFound);

        cage.UpdateSacrificableStatus();

        OrderRequest? orderRequest = null;
        if (orderRequestId.HasValue)
        {
            orderRequest = await _unitOfWork.OrderRequestRepository.GetByIdAsync(orderRequestId.Value);
            if (orderRequest == null)
                return Result.Failure<Sacrification>(OrderRequestErrors.NotFound);
        }

        var createResult = Sacrification.Create(cage, amount, sacrificationReason, orderRequest);
        if (createResult.IsFailure)
            return Result.Failure<Sacrification>(createResult.Error);

        // Now reduce offspring (this should work since Create validated it)
        var reduceResult = cage.ReduceOffspringsForSacrification(amount);
        if (reduceResult.IsFailure)
            return Result.Failure<Sacrification>(reduceResult.Error);

        // Save changes
        await _unitOfWork.CageRepository.UpdateAsync(cage);

        var sacrification = createResult.Value;
        var createdSacrification = await _unitOfWork.SacrificationRepository.AddAsync(sacrification);

        if (sacrification.SacrificationReason == SacrificationReason.Order)
            await _domainEventDispatcher.DispatchEventsAsync(sacrification.DomainEvents);

        return Result.Success(createdSacrification);
    }

    public async Task<Result<Sacrification>> GetSacrificationById(int sacrificationId)
    {
        var sacrification = await _unitOfWork.SacrificationRepository.GetByIdAsync(sacrificationId);
        if (sacrification == null)
            return Result.Failure<Sacrification>(SacrificationErrors.NotFound);

        return Result.Success(sacrification);
    }

    public async Task<Result<PaginatedResult<Sacrification>>> GetPaginatedSacrifications(
        PaginatedRequest<SacrificationFilterDto> request)
    {
        var sacrifications = await _unitOfWork.SacrificationRepository.GetPaginatedAsync(request);

        return Result.Success(sacrifications);
    }
}
