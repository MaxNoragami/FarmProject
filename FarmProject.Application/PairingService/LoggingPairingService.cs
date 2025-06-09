using FarmProject.Application.Common;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public class LoggingPairingService(
        IPairingService pairingService,
        LoggingHelper loggingHelper)
    : IPairingService
{
    private readonly IPairingService _pairingService = pairingService;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<Pair>> CreatePair(int breedingRabbitId, int maleRabbitId)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(CreatePair),
                (nameof(breedingRabbitId), breedingRabbitId),
                (nameof(maleRabbitId), maleRabbitId)
            ),
            async () =>
                await _pairingService
                    .CreatePair(breedingRabbitId, maleRabbitId));

    public async Task<Result<List<Pair>>> GetAllPairs()
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(nameof(GetAllPairs)),
            GetAllPairs);

    public async Task<Result<Pair>> GetPairById(int pairId)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(GetPairById),
                (nameof(pairId), pairId)
            ),
            async () =>
                await _pairingService
                    .GetPairById(pairId));

    public async Task<Result<Pair>> UpdatePairingStatus(int pairId, PairingStatus pairingStatus)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(UpdatePairingStatus),
                (nameof(pairId), pairId),
                (nameof(pairingStatus), pairingStatus)
            ),
            async () =>
                await _pairingService
                    .UpdatePairingStatus(pairId, pairingStatus));
}
