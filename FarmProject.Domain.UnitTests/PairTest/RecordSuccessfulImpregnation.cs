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
            var rabbitMale = new BreedingRabbit("John", Gender.Male);
            var rabbitFemale = new BreedingRabbit("Mary", Gender.Female);
            var endPairingDate = DateTime.Today;

            rabbitMale.SetBreedingStatus(BreedingStatus.Paired);
            rabbitMale.SetBreedingStatus(BreedingStatus.Paired);
            
            var pair = new Pair(rabbitMale, rabbitFemale, DateTime.Now);

            var pairingResult = pair.RecordSuccessfulImpregnation(endPairingDate);

            Assert.Multiple(
                () => Assert.True(pairingResult.IsSuccess),
                () => Assert.Equal(PairingStatus.Successful, pair.PairingStatus),
                () => Assert.Equal(BreedingStatus.Available, pair.MaleBreedingRabbit.BreedingStatus),
                () => Assert.Equal(BreedingStatus.Pregnant, pair.FemaleBreedingRabbit.BreedingStatus),
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

            var pairingResult = pair.RecordSuccessfulImpregnation(DateTime.Now);

            Assert.True(pairingResult.IsFailure);
            Assert.Equal(invalidPairingStatus, pair.PairingStatus);
        }
    }
}