using FarmProject.Application.Common;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.BirthService;

public class LoggingBirthService(
        IBirthService birthService,
        LoggingHelper loggingHelper)
    : IBirthService
{
    private readonly IBirthService _birthService = birthService;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<BreedingRabbit>> RecordBirth(int breedingRabbitId, int offspringCount)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(RecordBirth),
                (nameof(breedingRabbitId), breedingRabbitId),
                (nameof(offspringCount), offspringCount)
            ),
            async () =>
                await _birthService.RecordBirth(breedingRabbitId, offspringCount));

    public async Task<Result> SeparateOffspring(int currentCageId, int? otherCageId, int? femaleOffspringCount)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(SeparateOffspring),
                (nameof(currentCageId), currentCageId),
                (nameof(otherCageId), otherCageId),
                (nameof(femaleOffspringCount), femaleOffspringCount)
            ),
            async () =>
                await _birthService.SeparateOffspring(currentCageId, otherCageId, femaleOffspringCount));

    public async Task<Result> WeanOffspring(int oldCageId, int newCageId)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(WeanOffspring),
                (nameof(oldCageId), oldCageId),
                (nameof(newCageId), newCageId)
            ),
            async () =>
                await _birthService.WeanOffspring(oldCageId, newCageId));
}
