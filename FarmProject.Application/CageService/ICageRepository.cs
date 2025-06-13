using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.CageService;

public interface ICageRepository
{
    public Task<Cage> AddAsync(Cage cage);
    public Task<Cage?> GetByIdAsync(int cageId);
    public Task<List<Cage>> GetAllAsync();
    public Task<PaginatedResult<Cage>> GetPaginatedAsync(PaginatedRequest<CageFilterDto> request);
    public Task<List<Cage>> FindAsync(ISpecification<Cage> specification);
    public Task<Cage> UpdateAsync(Cage cage);
}
