using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.BreedingRabbitsService;

public class LoggingBreedingRabbitService(
        IBreedingRabbitService breedingRabbitService,
        LoggingHelper loggingHelper)
    : IBreedingRabbitService
{
    private readonly IBreedingRabbitService _breedingRabbitService = breedingRabbitService;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<BreedingRabbit>> AddBreedingRabbitToFarm(string name, int cageId)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(AddBreedingRabbitToFarm),
                (nameof(name), name),
                (nameof(cageId), cageId)
            ),
            async () =>
                await _breedingRabbitService
                    .AddBreedingRabbitToFarm(name, cageId));

    public async Task<Result<BreedingRabbit>> GetBreedingRabbitById(int breedingRabbitId)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetBreedingRabbitById), 
                (nameof(breedingRabbitId), breedingRabbitId)
            ),
            async () =>
                await _breedingRabbitService.GetBreedingRabbitById(breedingRabbitId));

    public async Task<Result<PaginatedResult<BreedingRabbit>>> GetPaginatedBreedingRabbits(
        PaginatedRequest<BreedingRabbitFilterDto> request
    )
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetPaginatedBreedingRabbits),
                (nameof(request), request)
            ),
            async () =>
                await _breedingRabbitService.GetPaginatedBreedingRabbits(request));

    public async Task<Result<BreedingRabbit>> UpdateBreedingStatus(int breedingRabbitId, BreedingStatus breedingStatus)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(UpdateBreedingStatus),
                (nameof(breedingRabbitId), breedingRabbitId),
                (nameof(breedingStatus), breedingStatus)
            ),
            async () =>
                await _breedingRabbitService
                    .UpdateBreedingStatus(breedingRabbitId, breedingStatus));
}
