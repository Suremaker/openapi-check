using Microsoft.AspNetCore.Mvc;

namespace WebApiV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponseContentController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        public string Get() => "OK";
    }
}