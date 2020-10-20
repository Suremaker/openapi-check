using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApiV2.Controllers
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
            public Guid RequestId { get; set; }
            public IDictionary<string, RecursiveRequestItem> OtherRequests { get; set; }
            public RecursiveRequestItem[] OtherItems { get; set; }
        }

        public class RecursiveRequestItem
        {
            public string Key { get; set; }
            public RecursiveRequest Item { get; set; }
        }

        public class RecursiveResponse
        {
            public Guid ResponseId { get; set; }
            public IDictionary<string, RecursiveResponseItem> OtherResponses { get; set; }
            public RecursiveResponseItem[] OtherItems { get; set; }
        }

        public class RecursiveResponseItem
        {
            public string Key { get; set; }
            public RecursiveResponse Item { get; set; }
        }
    }
}