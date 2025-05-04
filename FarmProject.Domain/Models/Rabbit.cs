using FarmProject.Domain.Constants;

namespace FarmProject.Domain.Models;
public class Rabbit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public BreedingStatus BreedingStatus { get; set; }
}
