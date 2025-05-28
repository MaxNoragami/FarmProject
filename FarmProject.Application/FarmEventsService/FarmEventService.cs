using FarmProject.Application.EventsService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.FarmEventsService;

public class FarmEventService(IUnitOfWork unitOfWork) : IFarmEventService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<FarmEvent>> CreateFarmEvent(FarmEvent farmEvent)
    {
        if (farmEvent == null)
            return Result.Failure<FarmEvent>(FarmEventErrors.NullValue);

        var createdEvent = await _unitOfWork.FarmEventRepository.AddAsync(farmEvent);
        return Result.Success(createdEvent);
    }

    public async Task<Result<List<FarmEvent>>> GetAllFarmEvents()
    {
        var allFarmEvents = await _unitOfWork.FarmEventRepository.GetAllAsync();
        return Result.Success(allFarmEvents);
    }

    public async Task<Result<List<FarmEvent>>> GetAllFarmEventsByDate(DateTime date)
    {
        var specification = new FarmEventSpecificationsByDate(date);
        var eventsOnDate = await _unitOfWork.FarmEventRepository.FindAsync(specification);
        return Result.Success(eventsOnDate);
    }

    public async Task<Result<List<FarmEvent>>> GetAllPendingFarmEvents()
    {
        var specification = new FarmEventSpecificationPending();
        var eventsPending = await _unitOfWork.FarmEventRepository.FindAsync(specification);
        return Result.Success(eventsPending);
    }

    public async Task<Result<FarmEvent>> GetFarmEventById(int eventId)
    {
        var requestEvent = await _unitOfWork.FarmEventRepository.GetByIdAsync(eventId);
        if (requestEvent == null)
            return Result.Failure<FarmEvent>(FarmEventErrors.NotFound);
        return Result.Success(requestEvent);
    }

    public async Task<Result<FarmEvent>> MarkFarmEventAsCompleted(int eventId)
    {
        var requestEvent = await _unitOfWork.FarmEventRepository.GetByIdAsync(eventId);

        if (requestEvent is null)
            return Result.Failure<FarmEvent>(FarmEventErrors.NotFound);

        var eventMarkCompletedResult = requestEvent.MarkAsCompleted();
        if (eventMarkCompletedResult.IsFailure)
            return Result.Failure<FarmEvent>(eventMarkCompletedResult.Error);

        var updateEvent = await _unitOfWork.FarmEventRepository.UpdateAsync(requestEvent);

        return Result.Success(updateEvent);
    }  
}
