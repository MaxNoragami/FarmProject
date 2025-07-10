using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.CageService;

public interface ICageService
{
    public Task<Result<Cage>> CreateCage(string name);
    public Task<Result<PaginatedResult<Cage>>> GetPaginatedCages(PaginatedRequest<CageFilterDto> request);
    public Task<Result<Cage>> GetCageById(int cageId);
    public Task<Result<Cage>> UpdateOffspringType(int cageId, OffspringType offspringType);
    public Task<Result<Cage>> MoveBreedingRabbitToCage(int breedingRabbitId, int destinationCageId);
    
}
