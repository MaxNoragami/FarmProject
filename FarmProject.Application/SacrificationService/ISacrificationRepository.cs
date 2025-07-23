using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.SacrificationService;

public interface ISacrificationRepository
{
    public Task<Sacrification> AddAsync(Sacrification sacrification);
    public Task<Sacrification?> GetByIdAsync(int sacrificationId);
    public Task<PaginatedResult<Sacrification>> GetPaginatedAsync(
        PaginatedRequest<SacrificationFilterDto> request);
    public Task<List<Sacrification>> FindAsync(ISpecification<Sacrification> specification);
    public Task<Sacrification> UpdateAsync(Sacrification sacrification);
}
