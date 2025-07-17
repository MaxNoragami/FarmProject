using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Application.Common.Models.Dtos;

public class OrderFilterDto : BaseEntityFilter<Order>
{
    public int? CustomerId { get; set; }
    public OrderStatus? OrderStatus { get; set; }
    public string? OrderDate { get; set; }

    public override IEnumerable<Expression<Func<Order, bool>>> GetExpressions()
    {
        var expressions = new List<Expression<Func<Order, bool>>>();

        if (CustomerId.HasValue)
            expressions.Add(
                order => order.CustomerId == CustomerId.Value);

        if (OrderStatus.HasValue)
            expressions.Add(
                order => order.OrderStatus == OrderStatus.Value);

        var orderDateValue = ParseDateString(OrderDate);

        if (orderDateValue.HasValue)
            expressions.Add(
                order => order.OrderDate.Date == orderDateValue.Value.Date);

        return expressions;
    }

    private static DateTime? ParseDateString(string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return null;

        if (DateTime.TryParse(dateString, out DateTime result))
            return result;

        return null;
    }
}
