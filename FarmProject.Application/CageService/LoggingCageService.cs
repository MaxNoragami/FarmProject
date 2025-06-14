using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.CageService;

public class LoggingCageService(
        ICageService cageService,
        LoggingHelper loggingHelper) 
    : ICageService
{
    private readonly ICageService _cageService = cageService;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<Cage>> AddOffspringsToCage(int cageId, int count)
        => await _loggingHelper.LogOperation(
                    LoggingUtilities.FormatMethodCall(
                        nameof(AddOffspringsToCage),
                        (nameof(cageId), cageId),
                        (nameof(count), count)
                    ),
                    async () =>
                        await _cageService.AddOffspringsToCage(cageId, count));

    public async Task<Result<Cage>> CreateCage(string name)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(nameof(CreateCage), (nameof(name), name)), 
            async () => 
                await _cageService.CreateCage(name));

    public async Task<Result<Cage>> GetCageById(int cageId)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(nameof(GetCageById), (nameof(cageId), cageId)),
            async () =>
                await _cageService.GetCageById(cageId));

    public async Task<Result<PaginatedResult<Cage>>> GetPaginatedCages(PaginatedRequest<CageFilterDto> request)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(nameof(GetPaginatedCages), (nameof(request), request)),
            async () =>
                await _cageService.GetPaginatedCages(request));

    public async Task<Result<Cage>> MoveBreedingRabbitToCage(int breedingRabbitId, int destinationCageId)
        => await _loggingHelper.LogOperation(
                LoggingUtilities.FormatMethodCall(
                    nameof(MoveBreedingRabbitToCage), 
                    (nameof(breedingRabbitId), breedingRabbitId), 
                    (nameof(destinationCageId), destinationCageId)
                ),
                async () =>
                    await _cageService.MoveBreedingRabbitToCage(breedingRabbitId, destinationCageId));

    public async Task<Result<Cage>> RemoveOffspringsFromCage(int cageId, int count)
        => await _loggingHelper.LogOperation(
                    LoggingUtilities.FormatMethodCall(
                        nameof(RemoveOffspringsFromCage),
                        (nameof(cageId), cageId),
                        (nameof(count), count)
                    ),
                    async () =>
                        await _cageService.RemoveOffspringsFromCage(cageId, count));

    public async Task<Result<Cage>> UpdateOffspringType(int cageId, OffspringType offspringType)
        => await _loggingHelper.LogOperation(
                    LoggingUtilities.FormatMethodCall(
                        nameof(UpdateOffspringType),
                        (nameof(cageId), cageId),
                        (nameof(offspringType), offspringType)
                    ),
                    async () =>
                        await _cageService.UpdateOffspringType(cageId, offspringType));
}
