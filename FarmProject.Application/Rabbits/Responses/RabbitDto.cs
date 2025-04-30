using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Rabbits.Responses;

public class RabbitDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public bool Breedable { get; set; }

    public static RabbitDto FromRabbit(Rabbit rabbit)
        => new RabbitDto()
        {
            Id = rabbit.Id,
            Name = rabbit.Name,
            Gender = rabbit.Gender,
            Breedable = rabbit.Breedable
        };
}
