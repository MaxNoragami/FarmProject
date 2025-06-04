using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.BreedingRabbitsService;

public interface IBreedingRabbitService
{
    public Task<Result<BreedingRabbit>> AddBreedingRabbitToFarm(string name, Gender gender, int cageId);
    public Task<Result<List<BreedingRabbit>>> GetAllBreedingRabbits();
    public Task<Result<BreedingRabbit>> GetBreedingRabbitById(int breedingRabbitId);
    public Task<Result<BreedingRabbit>> UpdateBreedingStatus(int breedingRabbitId, BreedingStatus breedingStatus);
}
