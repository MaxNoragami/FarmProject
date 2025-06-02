using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Domain.UnitTests.PairTest;

public class RecordFailedImpregnation
{
    [Fact]
    public void UpdatePairingFailure()
    {
        var rabbitMale = new BreedingRabbit("John", Gender.Male);
        var rabbitFemale = new BreedingRabbit("Mary", Gender.Female);
        var endPairingDate = DateTime.Today;

        rabbitMale.SetBreedingStatus(BreedingStatus.Paired);
        rabbitMale.SetBreedingStatus(BreedingStatus.Paired);

        var pair = new Pair(rabbitMale, rabbitFemale, endPairingDate);

        var pairingResult = pair.RecordFailedImpregnation(endPairingDate);

        Assert.Multiple(
            () => Assert.True(pairingResult.IsSuccess),
            () => Assert.Equal(PairingStatus.Failed, pair.PairingStatus),
            () => Assert.Equal(BreedingStatus.Available, pair.FemaleBreedingRabbit.BreedingStatus),
            () => Assert.Equal(BreedingStatus.Available, pair.MaleBreedingRabbit.BreedingStatus),
            () => Assert.Equal(endPairingDate, pair.EndDate)
        );
    }

    [Theory]
    [InlineData(PairingStatus.Successful)]
    [InlineData(PairingStatus.Failed)]
    public void DenoteChangeStatusOnOtherThanActivePairings(PairingStatus invalidPairingStatus)
    {
        var rabbitMale = new BreedingRabbit("John", Gender.Male);
        var rabbitFemale = new BreedingRabbit("Mary", Gender.Female);
        var endPairingDate = DateTime.Today;

        rabbitMale.SetBreedingStatus(BreedingStatus.Paired);
        rabbitMale.SetBreedingStatus(BreedingStatus.Paired);

        var pair = new Pair(rabbitMale, rabbitFemale, endPairingDate);

        pair.PairingStatus = invalidPairingStatus;

        var pairingResult = pair.RecordFailedImpregnation(endPairingDate);

        Assert.True(pairingResult.IsFailure);
        Assert.Equal(invalidPairingStatus, pair.PairingStatus);
    }
}
