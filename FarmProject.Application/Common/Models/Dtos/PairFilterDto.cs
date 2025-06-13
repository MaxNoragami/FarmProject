using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Application.Common.Models.Dtos;

public class PairFilterDto : BaseEntityFilter<Pair>
{
    public int? MaleRabbitId { get; set; }
    public int? FemaleRabbitId { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public PairingStatus? PairingStatus { get; set; }

    public override IEnumerable<Expression<Func<Pair, bool>>> GetExpressions()
    {
        var expressions = new List<Expression<Func<Pair, bool>>>();

        if (MaleRabbitId.HasValue)
            expressions.Add(
                pair => pair.MaleRabbitId == MaleRabbitId.Value);

        if (FemaleRabbitId.HasValue)
            expressions.Add(
                pair => pair.FemaleRabbit.Id == FemaleRabbitId.Value);

        if (PairingStatus.HasValue)
            expressions.Add(
                pair => pair.PairingStatus == PairingStatus.Value);

        var startDateValue = ParseDateString(StartDate);
        var endDateValue = ParseDateString(EndDate);

        if (startDateValue.HasValue)
            expressions.Add(pair => pair.StartDate.Date == startDateValue.Value.Date);

        if (endDateValue.HasValue)
            expressions.Add(pair => pair.EndDate != null && pair.EndDate.Value.Date == endDateValue.Value.Date);

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
