using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.Application.CageService;

public interface ICageService
{
    public Task<Result<Cage>> CreateCage(string name);
    public Task<Result<List<Cage>>> GetAllCages();
    public Task<Result<List<Cage>>> GetUnoccupiedCages();
    public Task<Result<Cage>> GetCageById(int cageId);
    public Task<Result<Cage>> AssignBreedingRabbitToCage(int cageId, int breedingRabbitId);
    public Task<Result<Cage>> RemoveBreedingRabbitFromCage(int cageId, Gender gender);
    public Task<Result<Cage>> AddOffspringsToCage (int cageId, int count);
    public Task<Result<Cage>> RemoveOffspringsFromCage(int cageId, int count);
    public Task<Result<Cage>> UpdateOffspringType(int cageId, OffspringType offspringType);
    public Task<Result<Cage>> MoveBreedingRabbitToCage(int breedingRabbitId, int destinationCageId);
    
}
