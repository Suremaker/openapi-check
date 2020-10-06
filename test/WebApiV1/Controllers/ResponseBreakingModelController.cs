using Microsoft.AspNetCore.Mvc;

namespace WebApiV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ResponseBreakingModelController : ControllerBase
    {
        [HttpGet]
        public string Get() => "Ok";

        [HttpGet("number")]
        public int GetNumber() => 5;

        [HttpGet("details")]
        public MyModel GetDetails() => new MyModel();

        [HttpGet("array")]
        public int[] GetArray()=>new int[0];

        public class MyModel
        {
            public string Text { get; set; }
        }
    }
}