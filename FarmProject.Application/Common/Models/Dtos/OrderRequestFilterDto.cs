using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Application.Common.Models.Dtos;

public class OrderRequestFilterDto : BaseEntityFilter<OrderRequest>
{
    public int? CageId { get; set; }
    public OffspringType? OffspringType { get; set; }
    public OrderRequestStatus? OrderRequestStatus { get; set; }

    public override IEnumerable<Expression<Func<OrderRequest, bool>>> GetExpressions()
    {
        var expressions = new List<Expression<Func<OrderRequest, bool>>>();

        if (CageId.HasValue)
            expressions.Add(
                or => or.Cage.Id == CageId.Value);

        if (OffspringType.HasValue)
            expressions.Add(
                or => or.OffspringType == OffspringType.Value);

        if (OrderRequestStatus.HasValue)
            expressions.Add(
                or => or.OrderRequestStatus == OrderRequestStatus.Value);

        return expressions;
    }
}
