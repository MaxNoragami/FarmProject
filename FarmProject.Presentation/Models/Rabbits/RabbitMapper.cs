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
}
