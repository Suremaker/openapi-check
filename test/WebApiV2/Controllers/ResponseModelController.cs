using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApiV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ResponseModelController : ControllerBase
    {
        [HttpGet]
        public TestDataResponse[] Get()
        {
            return new TestDataResponse[0];
        }

        public class TestDataResponse
        {
            //Removed nullability
            public Guid Id { get; set; }
            //Removed nullability
            [Required]
            public string Text { get; set; }
            //Precised type
            public int Number { get; set; }
            public string NewField { get; set; }
        }
    }
}
