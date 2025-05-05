using FarmProject.Domain.Constants;

namespace FarmProject.Domain.Models;
public class Rabbit : Entity
{
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public BreedingStatus BreedingStatus { get; set; }
}
