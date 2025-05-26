using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Domain.UnitTests.PairTest;

public class RecordFailedImpregnation
{
    [Fact]
    public void UpdatePairingFailure()
    {
        var rabbitMale = new Rabbit(1, "John", Gender.Male);
        var rabbitFemale = new Rabbit(2, "Mary", Gender.Female);
        var endPairingDate = DateTime.Today;

        rabbitMale.SetBreedingStatus(BreedingStatus.Paired);
        rabbitMale.SetBreedingStatus(BreedingStatus.Paired);

        var pair = new Pair(1, rabbitMale, rabbitFemale, endPairingDate);

        var pairingResult = pair.RecordFailedImpregnation(endPairingDate);

        Assert.Multiple(
            () => Assert.True(pairingResult.IsSuccess),
            () => Assert.Equal(PairingStatus.Failed, pair.PairingStatus),
            () => Assert.Equal(BreedingStatus.Available, pair.FemaleRabbit.BreedingStatus),
            () => Assert.Equal(BreedingStatus.Available, pair.MaleRabbit.BreedingStatus),
            () => Assert.Equal(endPairingDate, pair.EndDate)
        );
    }

    [Theory]
    [InlineData(PairingStatus.Successful)]
    [InlineData(PairingStatus.Failed)]
    public void DenoteChangeStatusOnOtherThanActivePairings(PairingStatus invalidPairingStatus)
    {
        var rabbitMale = new Rabbit(1, "John", Gender.Male);
        var rabbitFemale = new Rabbit(2, "Mary", Gender.Female);
        var endPairingDate = DateTime.Today;

        rabbitMale.SetBreedingStatus(BreedingStatus.Paired);
        rabbitMale.SetBreedingStatus(BreedingStatus.Paired);

        var pair = new Pair(1, rabbitMale, rabbitFemale, endPairingDate);

        pair.PairingStatus = invalidPairingStatus;

        var pairingResult = pair.RecordFailedImpregnation(endPairingDate);

        Assert.True(pairingResult.IsFailure);
        Assert.Equal(invalidPairingStatus, pair.PairingStatus);
    }
}
