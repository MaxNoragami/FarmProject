using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.RabbitsService;

public class RabbitService(IUnitOfWork unitOfWork) : IRabbitService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Rabbit>> CreateRabbit(string name, Gender gender)
    {
        var requestRabbit = new Rabbit(
            name: name,
            gender: gender
        );
        var createdRabbit = await _unitOfWork.RabbitRepository.AddAsync(requestRabbit);

        return Result.Success(createdRabbit);
    }

    public async Task<Result<List<Rabbit>>> GetAllRabbits()
    {
        var rabbits = await _unitOfWork.RabbitRepository.GetAllAsync();
        return Result.Success(rabbits);
    }

    public async Task<Result<Rabbit>> GetRabbitById(int rabbitId)
    {
        var requestRabbit = await _unitOfWork.RabbitRepository.GetByIdAsync(rabbitId);
        if (requestRabbit == null)
            return Result.Failure<Rabbit>(RabbitErrors.NotFound);

        return Result.Success(requestRabbit);
    }

    public async Task<Result<Rabbit>> UpdateBreedingStatus(int rabbitId, BreedingStatus breedingStatus)
    {
        var requestRabbit = await _unitOfWork.RabbitRepository.GetByIdAsync(rabbitId);
        if (requestRabbit == null)
            return Result.Failure<Rabbit>(RabbitErrors.NotFound);

        var setBreedStatusResult = requestRabbit.SetBreedingStatus(breedingStatus);
        if (setBreedStatusResult.IsFailure)
            return Result.Failure<Rabbit>(setBreedStatusResult.Error);

        var updatedRabbit = await _unitOfWork.RabbitRepository.UpdateAsync(requestRabbit);

        return Result.Success(updatedRabbit);
    }
}
