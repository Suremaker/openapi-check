using Microsoft.AspNetCore.Mvc;

namespace WebApiV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponseBreakingModelController : ControllerBase
    {
        [HttpGet]
        public string[] Get() => new[] { "Ok" };

        [HttpGet("number")]
        public double GetNumber() => 5.5;

        [HttpGet("details")]
        public MyModel GetDetails() => new MyModel();

        [HttpGet("array")]
        public string[] GetArray() => new string[0];

        public class MyModel
        {
            public string Text { get; set; }
        }
    }
}