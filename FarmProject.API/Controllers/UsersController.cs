using FarmProject.Application.IdentityService;
using FarmProject.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace FarmProject.API.Controllers;

[Route("api/users")]
public class UsersController(
        AppIdentityDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        IIdentityService identityService)
    : AppBaseController
{
    private readonly AppIdentityDbContext _context = context;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly IIdentityService _identityService = identityService;

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterUser registerUser)
    {
        // Create a new IdentityUser that will persist the user to the DB
        var identity = new IdentityUser { Email = registerUser.Email, UserName = registerUser.Email };
        var createdIdentity = await _userManager.CreateAsync(identity, registerUser.Password);

        // Add first and last names as claims to the user, which also need to be persisted
        var newClaims = new List<Claim>
        {
            new("FirstName", registerUser.FirstName),
            new("LastName", registerUser.LastName)
        };

        await _userManager.AddClaimsAsync(identity, newClaims);

        // We want to add the user to a role, if the user doesn't exist we want to create it
        if (registerUser.Role == Role.Logistics)
        {
            var role = await _roleManager.FindByNameAsync(Role.Logistics.ToString());
            if (role == null)
            {
                role = new IdentityRole(Role.Logistics.ToString());
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(identity, Role.Logistics.ToString());

            // Add the newly added role to the claims
            newClaims.Add(new Claim(ClaimTypes.Role, Role.Logistics.ToString()));
        }
        else
        {
            var role = await _roleManager.FindByNameAsync(Role.Worker.ToString());
            if (role == null)
            {
                role = new IdentityRole(Role.Worker.ToString());
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(identity, Role.Worker.ToString());

            // Add the newly added role to the claims
            newClaims.Add(new Claim(ClaimTypes.Role, Role.Worker.ToString()));
        }

        // Create a ClaimsIdentity to be used when generating a JWT
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, identity.Email 
                ?? throw new InvalidOperationException()),
            new Claim(JwtRegisteredClaimNames.Email, identity.Email 
                ?? throw new InvalidOperationException())
        });

        // Also add claims for first and last names
        claimsIdentity.AddClaims(newClaims);

        var token = _identityService.CreateSecurityToken(claimsIdentity);
        var response = new AuthenticationResult(_identityService.WriteToken(token));
        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginUser login)
    {
        // Verify user exists
        var user = await _userManager.FindByEmailAsync(login.Email);
        if (user is null)
            return BadRequest();

        // Verify the combination of email and password is correct
        var result = await _signInManager
            .CheckPasswordSignInAsync(user, login.Password, false);
        if (!result.Succeeded)
            return BadRequest("Could not sign in");

        // Get the user claims from DB, as they are needed on the token
        var claims = await _userManager.GetClaimsAsync(user);

        // Get roles of the users as well, for the token
        var roles = await _userManager.GetRolesAsync(user);

        // Create a JWT token for the user
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email 
                ?? throw new InvalidOperationException()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email 
                ?? throw new InvalidOperationException())
        });

        // Add claims and roles we got from DB
        claimsIdentity.AddClaims(claims);

        foreach (var role in roles)
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));

        // Create the token
        var token = _identityService.CreateSecurityToken(claimsIdentity);

        // Generate and send the response
        var response = new AuthenticationResult(_identityService.WriteToken(token));
        return Ok(response);
    }

    public enum Role
    {
        Logistics,
        Worker
    }

    public record RegisterUser(
        string Email, 
        string Password, 
        string FirstName, 
        string LastName, 
        Role Role);


    public record LoginUser(
        string Email,
        string Password);

    public record AuthenticationResult(
        string Token);
}