using HomeBudgetAPI.DTOs.MonthlyBudget;
using HomeBudgetAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeBudgetAPI.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class MonthlyBudgetController : ControllerBase
    {

        private readonly IMonthlyBudgetService _service;
        private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        public MonthlyBudgetController(IMonthlyBudgetService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetBudget([FromQuery]int year, [FromQuery] int month)
        {
            var res = await _service.GetBudgetAsync(year, month, UserId);
            return Ok(res);
        }


        [HttpPut]
        public async Task<IActionResult> SetBudget(MonthlyBudgetRequest request)
        {
            var res = await _service.SetBudgetAsync(request, UserId);
            return Ok(res);
        }

    }
}
