using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Validators;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.CageService;

public class ValidationCageService(
        ICageService inner,
        ValidationHelper validationHelper)
    : ICageService
{
    private readonly ICageService _inner = inner;
    private readonly ValidationHelper _validationHelper = validationHelper;

    public Task<Result<Cage>> CreateCage(string name)
        => _validationHelper.ValidateAndExecute(
                new CreateCageParam(name),
                () => _inner.CreateCage(name));

    public Task<Result<Cage>> GetCageById(int cageId)
        => _inner.GetCageById(cageId);

    public Task<Result<PaginatedResult<Cage>>> GetPaginatedCages(PaginatedRequest<CageFilterDto> request)
        => _validationHelper.ValidateAndExecute(
                new PaginatedRequestParam<CageFilterDto>(request),
                () => _inner.GetPaginatedCages(request));

    public Task<Result<Cage>> MoveBreedingRabbitToCage(int breedingRabbitId, int destinationCageId)
        => _inner.MoveBreedingRabbitToCage(breedingRabbitId, destinationCageId);

    public Task<Result<Cage>> SacrificeOffspring(int cageId, int count)
        => _validationHelper.ValidateAndExecute(
                new SacrificeOffspringParam(cageId, count),
                () => _inner.SacrificeOffspring(cageId, count));

    public Task<Result<Cage>> UpdateOffspringType(int cageId, OffspringType offspringType)
        => _validationHelper.ValidateAndExecute(
                new UpdateOffspringTypeParam(cageId, offspringType),
                () => _inner.UpdateOffspringType(cageId, offspringType));
}

public record CreateCageParam(string Name);
public record UpdateOffspringTypeParam(int CageId, OffspringType OffspringType);
public record SacrificeOffspringParam(int CageId, int Count);