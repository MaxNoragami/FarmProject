using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.RabbitsService;

public class RabbitService(IRepository<Rabbit> rabbitRepository) : IRabbitService
{
    private readonly IRepository<Rabbit> _rabbitRepository = rabbitRepository;

    public Task<Result<Rabbit>> CreateRabbit(string name, Gender gender)
    {
        var requestRabbit = new Rabbit(
            id: GetNextId(),
            name: name,
            gender: gender
        );

        var createdRabbit = _rabbitRepository.Create(requestRabbit);

        return Task.FromResult(Result.Success(createdRabbit));
    }

    public Task<Result<List<Rabbit>>> GetAllRabbits()
    {
        var rabbits = _rabbitRepository.GetAll();
        return Task.FromResult(Result.Success(rabbits));
    }

    public Task<Result<Rabbit>> GetRabbitById(int rabbitId)
    {
        var requestRabbit = _rabbitRepository.GetById(rabbitId);

        if (requestRabbit == null)
            return Task.FromResult(Result.Failure<Rabbit>(RabbitErrors.NotFound));

        return Task.FromResult(Result.Success(requestRabbit));
    }

    public Task<Result<Rabbit>> UpdateBreedingStatus(int rabbitId, BreedingStatus breedingStatus)
    {
        var requestRabbit = _rabbitRepository.GetById(rabbitId);

        if (requestRabbit == null)
            return Task.FromResult(Result.Failure<Rabbit>(RabbitErrors.NotFound));

        var setBreedStatusResult = requestRabbit.SetBreedingStatus(breedingStatus);

        if (setBreedStatusResult.IsFailure)
            return Task.FromResult(Result.Failure<Rabbit>(setBreedStatusResult.Error));

        var updatedRabbit = _rabbitRepository.Update(requestRabbit);
        return Task.FromResult(Result.Success(updatedRabbit));
    }

    public Task<Result<Rabbit>> UpdateRabbit(Rabbit rabbit)
    {
        if (rabbit == null)
            return Task.FromResult(Result.Failure<Rabbit>(RabbitErrors.NotFound));

        var updatedRabbit = _rabbitRepository.Update(rabbit);
        return Task.FromResult(Result.Success(updatedRabbit));
    }

    private int GetNextId()
        => _rabbitRepository.GetLastId();
}
