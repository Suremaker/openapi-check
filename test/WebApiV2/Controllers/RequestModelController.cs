using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApiV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RequestModelController : ControllerBase
    {
        [HttpPost]
        public void PostModel([FromBody] RequestModel request)
        {
        }

        public class RequestModel
        {
            public Guid? RequestId { get; set; }
            public string Text { get; set; }
            public int? Number { get; set; }
            public string AdditionalParameter { get; set; }
        }
    }
}