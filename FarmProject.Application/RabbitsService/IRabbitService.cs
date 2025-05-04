using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.RabbitsService;

public interface IRabbitService
{
    public Task<Rabbit> CreateRabbit(string name, Gender gender, bool breedable);
    public Task<List<Rabbit>> GetAllRabbits();
    public Task<Rabbit> GetRabbitById(int rabbitId);
}
