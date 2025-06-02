using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Application.BreedingRabbitsService;

public class BreedingRabbitService(IUnitOfWork unitOfWork) : IBreedingRabbitService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<BreedingRabbit>> CreateBreedingRabbit(string name, Gender gender)
    {
        var requestBreedingRabbit = new BreedingRabbit(
            name: name,
            gender: gender
        );
        var createdBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.AddAsync(requestBreedingRabbit);

        return Result.Success(createdBreedingRabbit);
    }

    public async Task<Result<List<BreedingRabbit>>> GetAllBreedingRabbits()
    {
        var breedingRabbits = await _unitOfWork.BreedingRabbitRepository.GetAllAsync();
        return Result.Success(breedingRabbits);
    }

    public async Task<Result<BreedingRabbit>> GetBreedingRabbitById(int breedingRabbitId)
    {
        var requestBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
        if (requestBreedingRabbit == null)
            return Result.Failure<BreedingRabbit>(BreedingRabbitErrors.NotFound);

        return Result.Success(requestBreedingRabbit);
    }

    public async Task<Result<BreedingRabbit>> UpdateBreedingStatus(int breedingRabbitId, BreedingStatus breedingStatus)
    {
        var requestBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.GetByIdAsync(breedingRabbitId);
        if (requestBreedingRabbit == null)
            return Result.Failure<BreedingRabbit>(BreedingRabbitErrors.NotFound);

        var setBreedStatusResult = requestBreedingRabbit.SetBreedingStatus(breedingStatus);
        if (setBreedStatusResult.IsFailure)
            return Result.Failure<BreedingRabbit>(setBreedStatusResult.Error);

        var updatedBreedingRabbit = await _unitOfWork.BreedingRabbitRepository.UpdateAsync(requestBreedingRabbit);

        return Result.Success(updatedBreedingRabbit);
    }
}
