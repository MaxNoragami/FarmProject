using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.RabbitsService;

public class RabbitService(IRabbitRepository rabbitRepository) : IRabbitService
{
    private readonly IRabbitRepository _rabbitRepository = rabbitRepository;

    public Task<Rabbit> CreateRabbit(string name, Gender gender, bool breedable)
    {
        var requestRabbit = new Rabbit()
        {
            Id = GetNextId(),
            Name = name,
            Gender = gender,
            Breedable = breedable
        };

        var createdRabbit = _rabbitRepository.Create(requestRabbit);

        return Task.FromResult(createdRabbit);
    }

    public Task<List<Rabbit>> GetAllRabbits()
    {
        var rabbits = _rabbitRepository.GetAll();
        return Task.FromResult(rabbits);
    }

    public Task<Rabbit> GetRabbitById(int rabbitId)
    {
        var requestRabbit = _rabbitRepository.GetById(rabbitId)
            ?? throw new ArgumentException("Rabbit not found.");

        return Task.FromResult(requestRabbit);
    }

    private int GetNextId()
        => _rabbitRepository.GetLastId();
}
