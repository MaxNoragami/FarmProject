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
            var rabbitMale = new Rabbit(1, "John", Gender.Male);
            var rabbitFemale = new Rabbit(2, "Mary", Gender.Female);
            var endPairingDate = DateTime.Today;

            rabbitMale.SetBreedingStatus(BreedingStatus.Paired);
            rabbitMale.SetBreedingStatus(BreedingStatus.Paired);
            
            var pair = new Pair(1, rabbitMale, rabbitFemale, DateTime.Now);

            var pairingResult = pair.RecordSuccessfulImpregnation(endPairingDate);

            Assert.Multiple(
                () => Assert.True(pairingResult.IsSuccess),
                () => Assert.Equal(PairingStatus.Successful, pair.PairingStatus),
                () => Assert.Equal(BreedingStatus.Available, pair.MaleRabbit.BreedingStatus),
                () => Assert.Equal(BreedingStatus.Pregnant, pair.FemaleRabbit.BreedingStatus),
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

            var pairingResult = pair.RecordSuccessfulImpregnation(DateTime.Now);

            Assert.True(pairingResult.IsFailure);
            Assert.Equal(invalidPairingStatus, pair.PairingStatus);
        }

        [Fact]
        public void AddEventOnSuccessfulImpregnation()
        {
            var rabbitMale = new Rabbit(1, "John", Gender.Male);
            var rabbitFemale = new Rabbit(2, "Mary", Gender.Female);

            rabbitMale.SetBreedingStatus(BreedingStatus.Paired);
            rabbitMale.SetBreedingStatus(BreedingStatus.Paired);

            var pair = new Pair(1, rabbitMale, rabbitFemale, DateTime.Now);

            pair.PairingStatus = PairingStatus.Active;
            var pairingResult = pair.RecordSuccessfulImpregnation(DateTime.Now);

            Assert.True(pairingResult.IsSuccess);
            Assert.Single(pair.FarmEvents);
            Assert.Equal(FarmEventType.NestPreparation, pair.FarmEvents[0].FarmEventType);
        }
    }
}