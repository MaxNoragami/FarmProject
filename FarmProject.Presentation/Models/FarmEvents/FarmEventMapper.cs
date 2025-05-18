using FarmProject.Domain.Models;

namespace FarmProject.Presentation.Models.FarmEvents;

public static class FarmEventMapper
{
    public static ViewFarmEventDto ToViewFarmEventDto(this FarmEvent farmEvent)
        => new ViewFarmEventDto()
        {
            Id = farmEvent.Id,
            FarmEventType = farmEvent.FarmEventType,
            Message = farmEvent.Message,
            IsCompleted = farmEvent.IsCompleted,
            CreatedOn = farmEvent.CreatedOn,
            DueOn = farmEvent.DueOn
        };
}
