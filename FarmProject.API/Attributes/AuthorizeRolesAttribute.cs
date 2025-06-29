using FarmProject.Application.IdentityService;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace FarmProject.API.Attributes;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params UserRole[] roles)
    {
        Roles = string.Join(",", roles.Select(r => r.ToString()));
    }
}
