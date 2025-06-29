using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FarmProject.Application.IdentityService;

public interface IIdentityService
{
    public SecurityToken CreateSecurityToken(ClaimsIdentity identity);
    public string WriteToken(SecurityToken token);
    public Task<AuthenticationResult> GenerateAuthenticationResultAsync(
        string email,
        IEnumerable<Claim> claims,
        IEnumerable<string> roles);
}
