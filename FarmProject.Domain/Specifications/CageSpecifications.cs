using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Domain.Specifications;

public class CageSpecificationByUnoccupied() : ISpecification<Cage>
{
    public Expression<Func<Cage, bool>> ToExpression()
        => cage =>
            cage.OffspringCount == 0
            && cage.BreedingRabbit == null;
}
