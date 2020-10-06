using Microsoft.AspNetCore.Mvc;

namespace WebApiV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsesController : ControllerBase
    {
        [HttpGet]
        public string Get() => "OK";

        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public int Post() => 123;
    }
}