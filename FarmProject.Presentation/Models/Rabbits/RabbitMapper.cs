using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;

namespace FarmProject.Presentation.Models.Rabbits;

public static class RabbitMapper
{
    public static ViewRabbitDto ToViewRabbitDto(this Rabbit rabbit)
        => new ViewRabbitDto()
            {
                Id = rabbit.Id,
                Name = rabbit.Name,
                Gender = rabbit.Gender,
                BreedingStatus = rabbit.BreedingStatus
            };

    public static Result<Rabbit> ToRabbit(this ViewRabbitDto rabbitDto)
    {

        var createdRabbit = new Rabbit(
            id: rabbitDto.Id,
            name: rabbitDto.Name,
            gender: rabbitDto.Gender
        );

        var updateBreedStatusResult = createdRabbit.SetBreedingStatus(rabbitDto.BreedingStatus);
        if (updateBreedStatusResult.IsFailure)
            return Result.Failure<Rabbit>(RabbitErrors.InvalidBreedingStatus);

        return Result.Success(createdRabbit);
    }
}
