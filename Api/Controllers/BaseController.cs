

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class BaseController : ControllerBase
{

}