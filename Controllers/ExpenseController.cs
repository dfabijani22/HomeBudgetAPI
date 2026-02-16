using Azure.Core;
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
        private readonly ApplicationDbContext _context;
        public ExpenseController(IExpenseService expenseService, ApplicationDbContext context) { 
            _expenseService = expenseService;
            _context = context;
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

        [HttpPatch("{expenseId}")]
        public async Task<IActionResult> UpdateExpense(int expenseId, [FromBody]ExpenseRequest request)
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var result = await _expenseService.UpdateExpenseAsync(userId, expenseId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);

        }

        [HttpDelete("{expenseId}")]
        public async Task<IActionResult> DeleteExpense(int expenseId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var result = await _expenseService.DeleteExpenseAsync(userId, expenseId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
