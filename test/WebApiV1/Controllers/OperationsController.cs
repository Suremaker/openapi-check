using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WebApiV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetAll() => new string[0];

        [HttpGet("{0}")]
        public string Get() => "Hi";

        [HttpPost]
        public string PostHello() => "Hi";
    }
}
