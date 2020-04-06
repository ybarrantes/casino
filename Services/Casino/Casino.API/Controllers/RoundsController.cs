
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Casino.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/rounds")]
    public class RoundsController : ControllerBase
    {

    }
}
