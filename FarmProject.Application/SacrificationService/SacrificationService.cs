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
        ICageService cageService,
        DomainEventDispatcher domainEventDispatcher) 
    : ISacrificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICageService _cageService = cageService;
    private readonly DomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;

    public async Task<Result<Sacrification>> SacrificeOffspring(
        int cageId, int amount, SacrificationReason sacrificationReason, int? orderRequestId)
    {
        var cageResult = await _cageService.SacrificeOffspring(cageId, amount);
        if (cageResult.IsFailure)
            return Result.Failure<Sacrification>(cageResult.Error);

        var cage = cageResult.Value;
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
        
        if (createResult.Value.SacrificationReason == SacrificationReason.Order)
            await _domainEventDispatcher.DispatchEventsAsync(cage.DomainEvents);

        return Result.Success(createResult.Value);
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
