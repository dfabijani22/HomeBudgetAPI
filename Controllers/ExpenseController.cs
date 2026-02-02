using HomeBudgetAPI.Data;
using HomeBudgetAPI.Models;
using HomeBudgetAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeBudgetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService) { 
            _expenseService = expenseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _expenseService.AddExpenseAsync(request, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserExpenses([FromQuery] int? month, [FromQuery] int? categoryId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var expenses = await _expenseService.GetUserExpenses(userId, month, categoryId);

            return Ok(expenses);
        }
    }
}
