using Microsoft.AspNetCore.Mvc;

namespace WebApiV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponseContentController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json", "application/xml")]
        public string Get() => "OK";
    }
}