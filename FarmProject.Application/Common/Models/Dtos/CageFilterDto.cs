using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Application.Common.Models.Dtos;

public class CageFilterDto : BaseEntityFilter<Cage>
{
    public string? Name { get; set; }
    public bool? IsOccupied { get; set; }
    public OffspringType? OffspringType { get; set; }

    public override IEnumerable<Expression<Func<Cage, bool>>> GetExpressions()
    {
        var expressions = new List<Expression<Func<Cage, bool>>>();

        if (!string.IsNullOrEmpty(Name))
            expressions.Add(
                cage => cage.Name.Contains(Name));

        if (IsOccupied.HasValue)
            expressions.Add(
                cage => (IsOccupied.Value)
                    ? cage.BreedingRabbit != null && 
                        cage.OffspringCount > 0 &&
                        cage.OffspringType != Domain.Constants.OffspringType.None

                    : cage.BreedingRabbit == null && 
                        cage.OffspringCount == 0 && 
                        cage.OffspringType == Domain.Constants.OffspringType.None);

        if (OffspringType.HasValue)
            expressions.Add(
                cage => cage.OffspringType == OffspringType.Value);

        return expressions;
    }
}
