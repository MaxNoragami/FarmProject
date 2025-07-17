using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;
using FarmProject.Domain.Constants;

namespace FarmProject.Application.SacrificationService;

public interface ISacrificationService
{
    public Task<Result<Sacrification>> SacrificeOffspring(
        int cageId, int amount, SacrificationReason sacrificationReason, int? orderRequestId);
    public Task<Result<PaginatedResult<Sacrification>>> GetPaginatedSacrifications(
        PaginatedRequest<SacrificationFilterDto> request);
    public Task<Result<Sacrification>> GetSacrificationById(int sacrificationId);
}
