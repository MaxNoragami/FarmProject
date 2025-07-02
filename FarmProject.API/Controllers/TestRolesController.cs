using FarmProject.API.Attributes;
using FarmProject.Application.IdentityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace FarmProject.API.Controllers;

[Route("api/test")]
[Authorize]
public class TestRolesController : AppBaseController
{
    [HttpGet]
    [AuthorizeRoles(UserRole.Worker, UserRole.Logistics)]
    public async Task<IActionResult> TestLogisticsWorkerRole()
        => Ok();

    [HttpGet]
    [Route("logistics")]
    [AuthorizeRoles(UserRole.Logistics)]
    public async Task<IActionResult> TestLogisticsRole()
        => Ok();

    [HttpGet]
    [Route("worker")]
    [AuthorizeRoles(UserRole.Worker)]
    public async Task<IActionResult> TestWorkerRole()
        => Ok();
}
