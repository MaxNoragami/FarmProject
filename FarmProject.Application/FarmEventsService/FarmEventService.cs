using FarmProject.Application.EventsService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Models;

namespace FarmProject.Application.FarmEventsService;

public class FarmEventService(IRepository<FarmEvent> farmEventsRepository) : IFarmEventService
{
    private readonly IRepository<FarmEvent> _farmEventsRepository = farmEventsRepository;

    public Task<Result<FarmEvent>> CreateFarmEvent(FarmEvent farmEvent)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<FarmEvent>>> GetAllFarmEvents()
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<FarmEvent>>> GetAllFarmEventsByDate()
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<FarmEvent>>> GetAllPendingFarmEvents()
    {
        throw new NotImplementedException();
    }

    public Task<Result<FarmEvent>> GetFarmEventById(int eventId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<FarmEvent>> MarkFarmEventAsCompleted(int eventId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<int>> GetNextAvailableEventId()
    {
        try
        {
            int nextId = _farmEventsRepository.GetLastId();
            return Task.FromResult(Result.Success(nextId));
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result.Failure<int>(
                new Error("FarmEvent.IdGenerationFailed", ex.Message)));
        }
    }    
}
