using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApiV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RecursiveModelController : ControllerBase
    {
        [HttpPost]
        public RecursiveResponse PostModel([Required][FromBody] RecursiveRequest request) => new RecursiveResponse();

        public class RecursiveRequest
        {
            public IDictionary<string,RecursiveRequestItem> OtherRequests { get; set; }
            public RecursiveRequestItem[] OtherItems { get; set; }
        }

        public class RecursiveRequestItem
        {
            public RecursiveRequest Item { get; set; }
        }

        public class RecursiveResponse
        {
            public IDictionary<string, RecursiveResponseItem> OtherResponses { get; set; }
            public RecursiveResponseItem[] OtherItems { get; set; }
        }

        public class RecursiveResponseItem
        {
            public RecursiveResponse Item { get; set; }
        }
    }
}