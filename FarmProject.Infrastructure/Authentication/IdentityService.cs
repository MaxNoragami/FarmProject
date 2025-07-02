using FarmProject.Application.IdentityService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FarmProject.Infrastructure.Authentication;

// A class for creating a JWT token
public class IdentityService : IIdentityService
{
    private readonly JwtSettings? _settings;
    private readonly byte[] _key;

    // Object that will generate the token
    private static JwtSecurityTokenHandler TokenHandler => new(); 

    public IdentityService(IOptions<JwtSettings> jwtOptions)
    {
        _settings = jwtOptions.Value;
        ArgumentNullException.ThrowIfNull(_settings);
        ArgumentNullException.ThrowIfNull(_settings.SigningKey);
        ArgumentNullException.ThrowIfNull(_settings.Audiences);
        ArgumentNullException.ThrowIfNull(_settings.Audiences[0]);
        ArgumentNullException.ThrowIfNull(_settings.Issuer);
        _key = Encoding.ASCII.GetBytes(_settings?.SigningKey!);
    }

    public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
    {
        var tokenDescriptor = GetTokenDescriptor(identity);
        return TokenHandler.CreateToken(tokenDescriptor);
    }

    public string WriteToken(SecurityToken token)
        => TokenHandler.WriteToken(token);

    // We describe our token
    // With the help of signing credentials we sign the token and using them we also validate it
    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
        => new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(2),
                Audience = _settings!.Audiences?[0]!,
                Issuer = _settings.Issuer,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

    public async Task<AuthenticationResult> GenerateAuthenticationResultAsync(
        string email,
        IEnumerable<Claim> claims,
        IEnumerable<string> roles)
    {
        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email)
        });

        claimsIdentity.AddClaims(claims);
        foreach (var role in roles)
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));

        var token = CreateSecurityToken(claimsIdentity);
        return new AuthenticationResult(WriteToken(token));
    }
}
