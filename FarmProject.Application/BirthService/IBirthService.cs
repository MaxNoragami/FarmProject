using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.BirthService;

public interface IBirthService
{
    public Task<Result<BreedingRabbit>> RecordBirth(int breedingRabbitId, int offspringCount);
    public Task<Result> WeanOffspring(int oldCageId, int newCageId);
    public Task<Result> SeparateOffspring(int currentCageId, int? otherCageId, int? femaleOffspringCount);
}
