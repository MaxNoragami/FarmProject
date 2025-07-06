using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Application.Common.Models.Dtos;

public class FarmTaskFilterDto : BaseEntityFilter<FarmTask>
{
    public FarmTaskType? FarmTaskType { get; set; }
    public bool? IsCompleted { get; set; }
    public string? CreatedOn { get; set; }
    public string? DueOn { get; set; }

    public override IEnumerable<Expression<Func<FarmTask, bool>>> GetExpressions()
    {
        var expressions = new List<Expression<Func<FarmTask, bool>>>();

        if (IsCompleted.HasValue)
            expressions.Add(
                farmTask => farmTask.IsCompleted == IsCompleted.Value);

        var createdOnValue = ParseDateString(CreatedOn);
        var dueOnValue = ParseDateString(DueOn);

        if (createdOnValue.HasValue)
            expressions.Add(farmTask => farmTask.CreatedOn.Date == createdOnValue.Value.Date);

        if (dueOnValue.HasValue)
            expressions.Add(farmTask => farmTask.DueOn.Date == dueOnValue.Value.Date);

        if (FarmTaskType.HasValue)
            expressions.Add(farmTask => farmTask.FarmTaskType == FarmTaskType.Value);
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
