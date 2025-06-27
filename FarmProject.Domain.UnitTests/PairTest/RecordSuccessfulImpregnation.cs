using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Domain.UnitTests.PairTest
{
    public class RecordSuccessfulImpregnation
    {
        [Fact]
        public void Record_SuccessfulImpregnation_SetsBreedingRabbitToPregnant()
        {
            var rabbitMaleId = 1;
            var rabbitFemale = new BreedingRabbit("Mary");
            var endPairingDate = DateTime.Today;

            rabbitFemale.BreedingStatus = BreedingStatus.Paired;

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
        public void RecordSuccessfulImpregnation_OnInvalidPairingStatus_ReturnsFailure(
            PairingStatus invalidPairingStatus)
        {
            var rabbitMaleId = 1;
            var rabbitFemale = new BreedingRabbit("Mary");
            var endPairingDate = DateTime.Today;

            rabbitFemale.BreedingStatus = BreedingStatus.Paired;

            var pair = new Pair(rabbitMaleId, rabbitFemale, endPairingDate);

            pair.PairingStatus = invalidPairingStatus;

            var pairingResult = pair.RecordSuccessfulImpregnation(DateTime.Now);

            Assert.True(pairingResult.IsFailure);
            Assert.Equal(invalidPairingStatus, pair.PairingStatus);
        }
    }
}