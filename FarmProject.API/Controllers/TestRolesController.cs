using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace FarmProject.API.Controllers;

[Route("api/test")]
[Authorize]
public class TestRolesController : AppBaseController
{
    [HttpGet]
    [Route("logistics-worker")]
    [Authorize(Roles = "Logistics, Worker")]
    public async Task<IActionResult> TestLogisticsWorkerRole()
        => Ok();

    [HttpGet]
    [Route("logistics")]
    [Authorize(Roles = "Logistics")]
    public async Task<IActionResult> TestLogisticsRole()
        => Ok();

    [HttpGet]
    [Route("worker")]
    [Authorize(Roles = "Worker")]
    public async Task<IActionResult> TestWorkerRole()
        => Ok();
}
