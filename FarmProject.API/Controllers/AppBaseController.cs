using FarmProject.API.ErrorHandling;
using FarmProject.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace FarmProject.API.Controllers;

[ApiController]
public abstract class AppBaseController : ControllerBase
{
    protected ActionResult<T> HandleError<T>(Error error) => error.ToActionResult<T>();
}
