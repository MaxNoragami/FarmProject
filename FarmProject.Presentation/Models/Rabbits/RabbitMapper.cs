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
                Breedable = rabbit.Breedable
            };

    public static Rabbit ToRabbit(this ViewRabbitDto rabbitDto)
        => new Rabbit()
        {
            Id = rabbitDto.Id,
            Name = rabbitDto.Name,
            Gender = rabbitDto.Gender,
            Breedable = rabbitDto.Breedable
        };
}
