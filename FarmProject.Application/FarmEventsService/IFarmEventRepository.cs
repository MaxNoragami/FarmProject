using FarmProject.Domain.Models;

namespace FarmProject.Application.FarmEventsService;

public interface IFarmEventRepository
{
    public Task<FarmEvent> AddAsync(FarmEvent farmEvent);
    public Task<FarmEvent?> GetByIdAsync(int farmEventId);
    public Task<List<FarmEvent>> GetAllAsync();
    public Task<FarmEvent> UpdateAsync(FarmEvent farmEvent);
    public Task RemoveAsync(FarmEvent farmEvent);
}
