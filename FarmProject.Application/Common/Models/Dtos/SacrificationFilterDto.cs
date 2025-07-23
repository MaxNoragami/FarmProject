using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Application.Common.Models.Dtos;

public class SacrificationFilterDto : BaseEntityFilter<Sacrification>
{
    public int? CageId { get; set; }
    public int? OrderRequestId { get; set; }
    public OffspringType? OffspringType { get; set; }
    public SacrificationReason? SacrificationReason { get; set; }

    public override IEnumerable<Expression<Func<Sacrification, bool>>> GetExpressions()
    {
        var expressions = new List<Expression<Func<Sacrification, bool>>>();

        if (CageId.HasValue)
            expressions.Add(
                s => s.Cage.Id == CageId.Value);

        if (OrderRequestId.HasValue)
            expressions.Add(
                s => s.OrderRequestId == OrderRequestId.Value);

        if (OffspringType.HasValue)
            expressions.Add(
                s => s.OffspringType == OffspringType.Value);

        if (SacrificationReason.HasValue)
            expressions.Add(
                s => s.SacrificationReason == SacrificationReason.Value);

        return expressions;
    }
}
