using FarmProject.Application.EventsService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.FarmEventsService;

public class FarmEventService(IRepository<FarmEvent> farmEventsRepository) : IFarmEventService
{
    private readonly IRepository<FarmEvent> _farmEventsRepository = farmEventsRepository;

    public Task<Result<FarmEvent>> CreateFarmEvent(FarmEvent farmEvent)
    {
        if (farmEvent == null)
            return Task.FromResult(Result.Failure<FarmEvent>(FarmEventErrors.NullValue));

        var createdEvent = _farmEventsRepository.Create(farmEvent);
        return Task.FromResult(Result.Success(createdEvent));
    }

    public Task<Result<List<FarmEvent>>> GetAllFarmEvents()
    {
        var allFarmEvents = _farmEventsRepository.GetAll();
        return Task.FromResult(Result.Success(allFarmEvents));
    }

    public Task<Result<List<FarmEvent>>> GetAllFarmEventsByDate(DateTime date)
    {
        var specification = new FarmEventSpecificationsByDate(date);
        var eventsOnDate = _farmEventsRepository.Find(specification);
        return Task.FromResult(Result.Success(eventsOnDate));
    }

    public Task<Result<List<FarmEvent>>> GetAllPendingFarmEvents()
    {
        var specification = new FarmEventSpecificationPending();
        var eventsPending = _farmEventsRepository.Find(specification);
        return Task.FromResult(Result.Success(eventsPending));
    }

    public Task<Result<FarmEvent>> GetFarmEventById(int eventId)
    {
        var requestEvent = _farmEventsRepository.GetById(eventId);
        if (requestEvent == null)
            return Task.FromResult(Result.Failure<FarmEvent>(FarmEventErrors.NotFound));
        return Task.FromResult(Result.Success(requestEvent));
    }

    public async Task<Result<FarmEvent>> MarkFarmEventAsCompleted(int eventId)
    {
        var requestEventResult = await GetFarmEventById(eventId);
        if (requestEventResult.IsFailure)
            return Result.Failure<FarmEvent>(requestEventResult.Error);

        var eventMarkCompletedResult = requestEventResult.Value.MarkAsCompleted();
        if (eventMarkCompletedResult.IsFailure)
            return Result.Failure<FarmEvent>(eventMarkCompletedResult.Error);

        var updateEventResult = await UpdateFarmEvent(requestEventResult.Value);
        if (updateEventResult.IsFailure)
            return Result.Failure<FarmEvent>(updateEventResult.Error);

        return Result.Success(updateEventResult.Value);
    }

    public Task<Result<FarmEvent>> UpdateFarmEvent(FarmEvent farmEvent)
    {
        if (farmEvent == null)
            return Task.FromResult(Result.Failure<FarmEvent>(FarmEventErrors.NotFound));

        var updatedEvent = _farmEventsRepository.Update(farmEvent);
        return Task.FromResult(Result.Success(updatedEvent));
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
