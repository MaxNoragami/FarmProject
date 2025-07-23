using FarmProject.Domain.Models;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace FarmProject.Application.Common.Models.Dtos;

public class CustomerFilterDto : BaseEntityFilter<Customer>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNum { get; set; }

    public override IEnumerable<Expression<Func<Customer, bool>>> GetExpressions()
    {
        var expressions = new List<Expression<Func<Customer, bool>>>();

        if (!string.IsNullOrEmpty(FirstName))
            expressions.Add(
                customer => customer.FirstName.Contains(FirstName));

        if (!string.IsNullOrEmpty(LastName))
            expressions.Add(
                customer => customer.LastName.Contains(LastName));

        if (!string.IsNullOrEmpty(Email))
            expressions.Add(
                customer => customer.Email.Contains(Email));

        if (!string.IsNullOrEmpty(PhoneNum))
            expressions.Add(
                customer => customer.PhoneNum.Contains(PhoneNum));

        return expressions;
    }
}
