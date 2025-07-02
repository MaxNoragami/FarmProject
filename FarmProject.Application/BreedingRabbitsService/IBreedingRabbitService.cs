using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.BreedingRabbitsService;

public interface IBreedingRabbitService
{
    public Task<Result<BreedingRabbit>> AddBreedingRabbitToFarm(string name, int cageId);
    public Task<Result<PaginatedResult<BreedingRabbit>>> GetPaginatedBreedingRabbits(
        PaginatedRequest<BreedingRabbitFilterDto> request);
    public Task<Result<BreedingRabbit>> GetBreedingRabbitById(int breedingRabbitId);
    public Task<Result<BreedingRabbit>> UpdateBreedingStatus(
        int breedingRabbitId, BreedingStatus breedingStatus);
}
