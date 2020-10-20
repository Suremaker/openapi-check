using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApiV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RequestBreakingModelController : ControllerBase
    {
        [HttpPost]
        public void PostModel([Required][FromBody] RequestBreakingModel request)
        {
        }

        public class RequestBreakingModel
        {
            public string SomeField { get; set; }
            public object[] SomeArray { get; set; }
        }
    }
}