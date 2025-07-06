using FarmProject.Application.Common;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.BirthService;

public class ValidationBirthService(
        IBirthService inner,
        ValidationHelper validationHelper)
    : IBirthService
{
    private readonly IBirthService _inner = inner;
    private readonly ValidationHelper _validationHelper = validationHelper;

    public Task<Result<BreedingRabbit>> RecordBirth(int breedingRabbitId, int offspringCount)
        => _validationHelper.ValidateAndExecute(
                new RecordBirthParam(breedingRabbitId, offspringCount),
                () => _inner.RecordBirth(breedingRabbitId, offspringCount));

    public Task<Result> SeparateOffspring(int currentCageId, int? otherCageId, int? femaleOffspringCount)
        => _validationHelper.ValidateAndExecute(
                new SeparateOffspringParam(currentCageId, otherCageId, femaleOffspringCount),
                () => _inner.SeparateOffspring(currentCageId, otherCageId, femaleOffspringCount));

    public Task<Result> WeanOffspring(int oldCageId, int newCageId)
        => _validationHelper.ValidateAndExecute(
                new WeanOffspringParam(oldCageId, newCageId),
                () => _inner.WeanOffspring(oldCageId, newCageId));
}

public record RecordBirthParam(int breedingRabbitId, int offspringCount);
public record SeparateOffspringParam(int currentCageId, int? otherCageId, int? femaleOffspringCount);
public record WeanOffspringParam(int oldCageId, int newCageId);