using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public static class PairErrors
{
    public static readonly Error NotFound = new(
        "Pair.NotFound", "Pair not found");

    public static readonly Error InvalidPairing = new(
        "Pair.InvalidPairing", "Those rabbits are not able to be paired");

    public static readonly Error InvalidStateChange = new(
        "Pair.InvalidStateChange", "Only active pairings can be completed");

    public static readonly Error InvalidOutcome = new(
        "Pair.InvalidOutcome", "Cannot set an active pairing as the outcome");

    public static readonly Error NotSuccessful = new(
        "Pair.NotSuccessful", "Cannot create nest preparation event for non-successful pairings");

    public static readonly Error NoEndDate = new(
        "Pair.NoEndDate", "Cannot create nest preparation for a pairing without an end date");
    
    public static readonly Error CreationFailed = new(
        "Pair.CreationFailed", "Failed to create a Pair");
}
