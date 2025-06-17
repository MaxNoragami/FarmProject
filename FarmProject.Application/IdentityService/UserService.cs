using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FarmProject.Application.IdentityService;

public class UserService(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        IIdentityService identityService)
    : IUserService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<AuthenticationResult>> RegisterUserAsync(RegisterUserRequest request)
    {
        // Check of already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return Result.Failure<AuthenticationResult>(IdentityErrors.UserAlreadyExists);

        // Create a new IdentityUser that will persist the user to the DB
        var identity = new IdentityUser
        {
            Email = request.Email,
            UserName = request.Email
        };

        var createdResult = await _userManager.CreateAsync(identity, request.Password);

        if (!createdResult.Succeeded)
            return Result.Failure<AuthenticationResult>(
                IdentityErrors.RegistrationFailed(
                    string.Join(", ", createdResult.Errors.Select(e => e.Description))));

        // Add first and last names as claims to the user, which also need to be persisted
        var newClaims = new List<Claim>
        {
            new Claim("FirstName", request.FirstName),
            new Claim("LastName", request.LastName)
        };

        await _userManager.AddClaimsAsync(identity, newClaims);

        // We want to add the user to a role, if the user doesn't exist we want to create it
        var roleName = request.Role.ToString();
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role == null)
        {
            role = new IdentityRole(roleName);
            await _roleManager.CreateAsync(role);
        }
        
        // Persist and add role to claims
        await _userManager.AddToRoleAsync(identity, roleName);
        newClaims.Add(new Claim(ClaimTypes.Role, roleName));

        // Create ClaimsIdentity, used for generating the JWT eventually
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
            new Claim(JwtRegisteredClaimNames.Email, identity.Email)
        });
        claimsIdentity.AddClaims(newClaims);

        var token = _identityService.CreateSecurityToken(claimsIdentity);
        return Result.Success(new AuthenticationResult(_identityService.WriteToken(token)));
    }

    public async Task<Result<AuthenticationResult>> LoginUserAsync(LoginUserRequest request)
    {
        // Find user by email
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result.Failure<AuthenticationResult>(IdentityErrors.InvalidCredentials);

        // Verify if the combo of email and pass is correct
        var result = await _signInManager
            .CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Result.Failure<AuthenticationResult>(IdentityErrors.InvalidCredentials);

        var claims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        // Create JWT for user
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        });

        claimsIdentity.AddClaims(claims);
        foreach (var role in roles)
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));

        var token = _identityService.CreateSecurityToken(claimsIdentity);
        return Result.Success(new AuthenticationResult(_identityService.WriteToken(token)));
    }
}
