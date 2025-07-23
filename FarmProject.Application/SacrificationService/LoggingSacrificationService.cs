using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.SacrificationService;

public class LoggingSacrificationService(
        ISacrificationService sacrificationService,
        LoggingHelper loggingHelper) 
    : ISacrificationService
{
    private readonly ISacrificationService _sacrificationService = sacrificationService;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<Sacrification>> SacrificeOffspring(
        int cageId, int amount, SacrificationReason sacrificationReason, int? orderRequestId
    )
        => await _loggingHelper.LogOperation(
                LoggingUtilities.FormatMethodCall(
                    nameof(SacrificeOffspring),
                    (nameof(cageId), cageId),
                    (nameof(amount), amount),
                    (nameof(sacrificationReason), sacrificationReason),
                    (nameof(orderRequestId), orderRequestId)
                ),
                async () =>
                    await _sacrificationService.SacrificeOffspring(
                        cageId, amount, sacrificationReason, orderRequestId));

    public async Task<Result<PaginatedResult<Sacrification>>> GetPaginatedSacrifications(
        PaginatedRequest<SacrificationFilterDto> request
    )
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetPaginatedSacrifications),
                (nameof(request), request)
            ),
            async () =>
                await _sacrificationService.GetPaginatedSacrifications(request));

    public async Task<Result<Sacrification>> GetSacrificationById(int sacrificationId)
        => await _loggingHelper.LogOperation(
                LoggingUtilities.FormatMethodCall(
                    nameof(GetSacrificationById),
                    (nameof(sacrificationId), sacrificationId)
                ),
                async () =>
                    await _sacrificationService.GetSacrificationById(sacrificationId));
}
