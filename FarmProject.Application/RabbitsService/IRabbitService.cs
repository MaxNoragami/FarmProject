using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.RabbitsService;

public interface IRabbitService
{
    public Task<Rabbit> CreateRabbit(string name, Gender gender, BreedingStatus breedingStatus);
    public Task<List<Rabbit>> GetAllRabbits();
    public Task<Rabbit> GetRabbitById(int rabbitId);
    public Task<Rabbit> UpdateBreedingStatus(int rabbitId, BreedingStatus breedingStatus);
}
