using FarmProject.Domain.Events;
using FarmProject.Domain.Models;
using FluentAssertions;

namespace FarmProject.Domain.UnitTests.RabbitTest;

public class Breed
{
    [Fact]
    public void VerifyCreationOfBreedEvent()
    {
        var femaleRabbit = new BreedingRabbit("Female")
        {
            Id = 1
        };
        var maleRabbitId = 2;

        var startBreedDate = DateTime.Now;

        femaleRabbit.Breed(maleRabbitId, startBreedDate);

        Assert.Single(femaleRabbit.DomainEvents);
        Assert.IsType<BreedEvent>(femaleRabbit.DomainEvents.First());
        femaleRabbit.DomainEvents.First()
            .Should()
            .BeOfType<BreedEvent>()
            .Subject.StartDate
            .Should()
            .Be(startBreedDate);

        femaleRabbit.DomainEvents.First()
            .Should()
            .BeOfType<BreedEvent>()
            .Subject.BreedingRabbitIds
            .Should()
            .Contain([1, 2]);
    }
}
