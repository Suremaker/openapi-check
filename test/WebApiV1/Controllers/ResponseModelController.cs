using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WebApiV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ResponseModelController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<TestDataResponse> Get()
        {
            return new TestDataResponse[0];
        }

        public class TestDataResponse
        {
            public Guid? Id { get; set; }
            public string Text { get; set; }
            public int Number { get; set; }
        }
    }
}
