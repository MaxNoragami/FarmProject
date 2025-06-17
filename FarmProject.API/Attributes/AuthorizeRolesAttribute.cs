using Microsoft.AspNetCore.Authorization;
using FarmProject.Domain.Identity;
using System.Data;

namespace FarmProject.API.Attributes;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params UserRole[] roles)
    {
        Roles = string.Join(",", roles.Select(r => r.ToString()));
    }
}
