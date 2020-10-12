using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApiV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RequestModelController : ControllerBase
    {
        [HttpPost]
        public void PostModel([Required][FromBody] RequestModel request)
        {
        }

        public class RequestModel
        {
            public Guid RequestId { get; set; }
            [Required]
            public string Text { get; set; }
            public int Number { get; set; }
        }
    }
}