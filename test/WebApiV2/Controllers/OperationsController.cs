using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WebApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetAll() => new string[0];

        [HttpGet("{0}")]
        public string Get() => "Hi";

        [HttpGet("{0}/summary")]
        public string GetSummary() => "Summary";

        [HttpPost]
        public string PostHello() => "Hi";

        [HttpPut]
        public string PutHello() => "Hi";

        [HttpGet("some")]
        [Obsolete]
        public IEnumerable<string> GetSome() => new string[0];
    }
}
