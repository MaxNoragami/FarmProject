using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Domain.UnitTests.PairTest;

public class RecordFailedImpregnation
{
    [Fact]
    public void UpdatePairingFailure()
    {
        var rabbitMaleId = 1;
        var rabbitFemale = new BreedingRabbit("Mary");
        var endPairingDate = DateTime.Today;

        rabbitFemale.BreedingStatus = BreedingStatus.Paired;

        var pair = new Pair(rabbitMaleId, rabbitFemale, endPairingDate);

        var pairingResult = pair.RecordFailedImpregnation(endPairingDate);

        Assert.Multiple(
            () => Assert.True(pairingResult.IsSuccess),
            () => Assert.Equal(PairingStatus.Failed, pair.PairingStatus),
            () => Assert.Equal(BreedingStatus.Available, pair.FemaleRabbit.BreedingStatus),
            () => Assert.Equal(endPairingDate, pair.EndDate)
        );
    }

    [Theory]
    [InlineData(PairingStatus.Successful)]
    [InlineData(PairingStatus.Failed)]
    public void DenoteChangeStatusOnOtherThanActivePairings(PairingStatus invalidPairingStatus)
    {
        var rabbitMaleId = 1;
        var rabbitFemale = new BreedingRabbit("Mary");
        var endPairingDate = DateTime.Today;

        rabbitFemale.BreedingStatus = BreedingStatus.Paired;

        var pair = new Pair(rabbitMaleId, rabbitFemale, endPairingDate);

        pair.PairingStatus = invalidPairingStatus;

        var pairingResult = pair.RecordFailedImpregnation(endPairingDate);

        Assert.True(pairingResult.IsFailure);
        Assert.Equal(invalidPairingStatus, pair.PairingStatus);
    }
}
