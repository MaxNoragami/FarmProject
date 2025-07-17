using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.SacrificationService;

public class SacrificationService(
        IUnitOfWork unitOfWork) 
    : ISacrificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public Task<Result<Sacrification>> SacrificeOffspring(
        int cageId, int amount, SacrificationReason sacrificationReason, int? orderRequestId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<PaginatedResult<Sacrification>>> GetPaginatedSacrifications(
        PaginatedRequest<SacrificationFilterDto> request)
    {
        var sacrifications = await _unitOfWork.SacrificationRepository.GetPaginatedAsync(request);

        return Result.Success(sacrifications);
    }

    public async Task<Result<Sacrification>> GetSacrificationById(int sacrificationId)
    {
        var sacrification = await _unitOfWork.SacrificationRepository.GetByIdAsync(sacrificationId);
        if (sacrification == null)
            return Result.Failure<Sacrification>(SacrificationErrors.NotFound);

        return Result.Success(sacrification);
    }
}
