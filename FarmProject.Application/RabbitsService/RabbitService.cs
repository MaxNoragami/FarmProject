using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using System.Reflection;

namespace FarmProject.Application.RabbitsService;

public class RabbitService(IRepository<Rabbit> rabbitRepository) : IRabbitService
{
    private readonly IRepository<Rabbit> _rabbitRepository = rabbitRepository;

    public Task<Result<Rabbit>> CreateRabbit(string name, Gender gender, BreedingStatus breedingStatus)
    {
        var validationResult = RabbitValidator.ValidateMakeBreedingStatus(gender, breedingStatus);

        if (validationResult.IsFailure)
            return Task.FromResult(Result.Failure<Rabbit>(RabbitErrors.InvalidBreedingStatus));

        var requestRabbit = new Rabbit()
        {
            Id = GetNextId(),
            Name = name,
            Gender = gender,
            BreedingStatus = breedingStatus
        };

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

        var validationResult = RabbitValidator.ValidateMakeBreedingStatus(requestRabbit.Gender, breedingStatus);

        if (validationResult.IsFailure)
            return Task.FromResult(Result.Failure<Rabbit>(RabbitErrors.InvalidBreedingStatus));

        requestRabbit.BreedingStatus = breedingStatus;

        var updatedRabbit = _rabbitRepository.Update(requestRabbit);
        return Task.FromResult(Result.Success(updatedRabbit));
    }

    private int GetNextId()
        => _rabbitRepository.GetLastId();
}
