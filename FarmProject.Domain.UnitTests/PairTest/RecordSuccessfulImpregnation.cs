using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using Xunit;

namespace FarmProject.Domain.UnitTests.PairTest
{
    public class RecordSuccessfulImpregnation
    {
        [Fact]
        public void UpdatePairingSuccess()
        {
            var rabbitMaleId = 1;
            var rabbitFemale = new BreedingRabbit("Mary");
            var endPairingDate = DateTime.Today;

            rabbitFemale.SetBreedingStatus(BreedingStatus.Paired);

            var pair = new Pair(rabbitMaleId, rabbitFemale, endPairingDate);

            var pairingResult = pair.RecordSuccessfulImpregnation(endPairingDate);

            Assert.Multiple(
                () => Assert.True(pairingResult.IsSuccess),
                () => Assert.Equal(PairingStatus.Successful, pair.PairingStatus),
                () => Assert.Equal(BreedingStatus.Pregnant, pair.FemaleRabbit.BreedingStatus),
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

            rabbitFemale.SetBreedingStatus(BreedingStatus.Paired);

            var pair = new Pair(rabbitMaleId, rabbitFemale, endPairingDate);

            pair.PairingStatus = invalidPairingStatus;

            var pairingResult = pair.RecordSuccessfulImpregnation(DateTime.Now);

            Assert.True(pairingResult.IsFailure);
            Assert.Equal(invalidPairingStatus, pair.PairingStatus);
        }
    }
}