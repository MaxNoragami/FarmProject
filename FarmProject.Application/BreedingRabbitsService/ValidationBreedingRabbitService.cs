using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.BreedingRabbitsService;

public class ValidationBreedingRabbitService(
        IBreedingRabbitService inner,
        ValidationHelper validationHelper)
    : IBreedingRabbitService
{
    private readonly IBreedingRabbitService _inner = inner;
    private readonly ValidationHelper _validationHelper = validationHelper;

    public Task<Result<BreedingRabbit>> AddBreedingRabbitToFarm(string name, int cageId)
        => _validationHelper.ValidateAndExecute(
                new AddBreedingRabbitParam(name, cageId),
                () => _inner.AddBreedingRabbitToFarm(name, cageId));

    public Task<Result<List<BreedingRabbit>>> GetAllAvailableBreedingRabbits()
    {
        throw new NotImplementedException();
    }

    public Task<Result<BreedingRabbit>> GetBreedingRabbitById(int breedingRabbitId)
        => _inner.GetBreedingRabbitById(breedingRabbitId);

    public Task<Result<PaginatedResult<BreedingRabbit>>> GetPaginatedBreedingRabbits(PaginatedRequest<BreedingRabbitFilterDto> request)
        => _inner.GetPaginatedBreedingRabbits(request);

    public Task<Result<BreedingRabbit>> UpdateBreedingStatus(int breedingRabbitId, BreedingStatus breedingStatus)
        => _inner.UpdateBreedingStatus(breedingRabbitId, breedingStatus);
}

public record AddBreedingRabbitParam(string Name, int CageId);
