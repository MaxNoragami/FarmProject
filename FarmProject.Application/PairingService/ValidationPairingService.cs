using FarmProject.Application.Common;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Validators;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public class ValidationPairingService(
        IPairingService inner,
        ValidationHelper validationHelper)
    : IPairingService
{
    private readonly IPairingService _inner = inner;
    private readonly ValidationHelper _validationHelper = validationHelper;

    public Task<Result<Pair>> CreatePair(int breedingRabbitId, int maleRabbitId)
        => _inner.CreatePair(breedingRabbitId, maleRabbitId);

    public Task<Result<PaginatedResult<Pair>>> GetPaginatedPairs(PaginatedRequest<PairFilterDto> request)
        => _validationHelper.ValidateAndExecute(
                new PaginatedRequestParam<PairFilterDto>(request),
                () => _inner.GetPaginatedPairs(request));

    public Task<Result<Pair>> GetPairById(int pairId)
        => _inner.GetPairById(pairId);

    public Task<Result<Pair>> UpdatePairingStatus(int pairId, PairingStatus pairingStatus)
        => _validationHelper.ValidateAndExecute(
                new UpdatePairingStatusParam(pairId, pairingStatus),
                () => _inner.UpdatePairingStatus(pairId, pairingStatus));
}

public record UpdatePairingStatusParam(int PairId, PairingStatus PairingStatus);