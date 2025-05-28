using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.FarmEventsService;

public interface IFarmEventRepository
{
    public Task<FarmEvent> AddAsync(FarmEvent farmEvent);
    public Task<FarmEvent?> GetByIdAsync(int farmEventId);
    public Task<List<FarmEvent>> GetAllAsync();
    public Task<List<FarmEvent>> FindAsync(ISpecification<FarmEvent> specification);
    public Task<FarmEvent> UpdateAsync(FarmEvent farmEvent);
    public Task RemoveAsync(FarmEvent farmEvent);
}
