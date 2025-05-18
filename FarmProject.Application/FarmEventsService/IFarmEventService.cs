using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.EventsService;

public interface IFarmEventService
{
    public Task<Result<FarmEvent>> CreateFarmEvent(FarmEvent farmEvent);
    public Task<Result<List<FarmEvent>>> GetAllFarmEvents();
    public Task<Result<List<FarmEvent>>> GetAllPendingFarmEvents();
    public Task<Result<List<FarmEvent>>> GetAllFarmEventsByDate();
    public Task<Result<FarmEvent>> GetFarmEventById(int eventId);
    public Task<Result<FarmEvent>> MarkFarmEventAsCompleted(int eventId);
    public Task<Result<int>> GetNextAvailableEventId();
}
