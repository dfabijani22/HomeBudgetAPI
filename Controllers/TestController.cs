using HomeBudgetAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudgetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var result = _testService.GetStatus();
            return Ok(result);
        }
    }
}
