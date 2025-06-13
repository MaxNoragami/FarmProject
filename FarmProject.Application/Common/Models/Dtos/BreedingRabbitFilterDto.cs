using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Application.Common.Models.Dtos;

public class BreedingRabbitFilterDto : BaseEntityFilter<BreedingRabbit>
{
    public string? Name { get; set; }
    public BreedingStatus? BreedingStatus { get; set; }

    public override IEnumerable<Expression<Func<BreedingRabbit, bool>>> GetExpressions()
    {
        var expressions = new List<Expression<Func<BreedingRabbit, bool>>>();

        if (!string.IsNullOrEmpty(Name))
            expressions.Add(
                breedingRabbit => breedingRabbit.Name.Contains(Name));

        if (BreedingStatus.HasValue)
            expressions.Add(
                breedingRabbit => breedingRabbit.BreedingStatus == BreedingStatus.Value);

        return expressions;
    }
}
