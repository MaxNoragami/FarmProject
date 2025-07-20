using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Validators;
using FarmProject.Application.OrderRequestService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.SacrificationService;

public class ValidationSacrificationService(
        ISacrificationService inner,
        ValidationHelper validationHelper) 
    : ISacrificationService
{
    private readonly ISacrificationService _inner = inner;
    private readonly ValidationHelper _validationHelper = validationHelper;

    public Task<Result<Sacrification>> SacrificeOffspring(
        int cageId, int amount, SacrificationReason sacrificationReason, int? orderRequestId
    )
        => _validationHelper.ValidateAndExecute(
            new SacrificeOffspringParam(cageId, amount, sacrificationReason, orderRequestId),
                () => _inner.SacrificeOffspring(
                    cageId, amount, sacrificationReason, orderRequestId));

    public Task<Result<PaginatedResult<Sacrification>>> GetPaginatedSacrifications(
        PaginatedRequest<SacrificationFilterDto> request
    )
        => _validationHelper.ValidateAndExecute(
            new PaginatedRequestParam<SacrificationFilterDto>(request),
                () => _inner.GetPaginatedSacrifications(request));

    public Task<Result<Sacrification>> GetSacrificationById(int sacrificationId)
        => _inner.GetSacrificationById(sacrificationId);
}

public record SacrificeOffspringParam(
    int CageId, int Amount, SacrificationReason SacrificationReason, int? OrderRequestId);