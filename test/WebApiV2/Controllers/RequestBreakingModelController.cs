using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApiV2.Controllers
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
           public int SomeField { get; set; }
           public int[] SomeArray { get; set; }
        }
    }
}