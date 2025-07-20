using FarmProject.API.Dtos.Orders;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.Sacrifications;

public static class SacrificationMapper
{
    public static ViewSacrificationDto ToViewSacrificationDto(this Sacrification sacrification)
        => new ViewSacrificationDto()
            {
                Id = sacrification.Id,
                SacrificationReason = sacrification.SacrificationReason,
                CageId = sacrification.Cage.Id,
                OffspringType = sacrification.OffspringType,
                Amount = sacrification.Amount,
                BirthDate = sacrification.BirthDate,
                OrderRequestId = sacrification.OrderRequestId
            };
}
