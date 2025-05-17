using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.RabbitsService;

public interface IRabbitService
{
    public Task<Result<Rabbit>> CreateRabbit(string name, Gender gender);
    public Task<Result<List<Rabbit>>> GetAllRabbits();
    public Task<Result<Rabbit>> GetRabbitById(int rabbitId);
    public Task<Result<Rabbit>> UpdateRabbit(Rabbit rabbit);
    public Task<Result<Rabbit>> UpdateBreedingStatus(int rabbitId, BreedingStatus breedingStatus);
}
