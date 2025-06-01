using FarmProject.Domain.Constants;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;
using FluentAssertions;

namespace FarmProject.Domain.UnitTests.RabbitTest;

public class Breed
{
    [Fact]
    public void VerifyCreationOfBreedEvent()
    {
        var femaleRabbit = new Rabbit("Female", Gender.Female)
        {
            Id = 1
        };
        var maleRabbit = new Rabbit("Male", Gender.Male)
        {
            Id = 2
        };
        var startBreedDate = DateTime.Now;

        femaleRabbit.Breed(maleRabbit, startBreedDate);

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
            .Subject.RabbitIds
            .Should()
            .Contain([1, 2]);
    }


    [Fact]
    public void BreedEventNotSuccessful()
    {
        var maleRabbit1 = new Rabbit("Female", Gender.Male)
        {
            Id = 1
        };
        var maleRabbit2 = new Rabbit("Male", Gender.Male)
        {
            Id = 2
        };
        var startBreedDate = DateTime.Now;

        maleRabbit1.Breed(maleRabbit2, startBreedDate);

        Assert.Empty(maleRabbit1.DomainEvents);
    }

    [Fact]
    public void VerifyOnlyOneRabbitHasEvents()
    {
        var femaleRabbit = new Rabbit("Female", Gender.Female)
        {
            Id = 1
        };
        var maleRabbit = new Rabbit("Male", Gender.Male)
        {
            Id = 2
        };
        var startBreedDate = DateTime.Now;

        femaleRabbit.Breed(maleRabbit, startBreedDate);

        Assert.Empty(maleRabbit.DomainEvents);
    }
}
