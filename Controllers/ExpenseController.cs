using Azure.Core;
using HomeBudgetAPI.Data;
using HomeBudgetAPI.DTOs.Expense;
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
        private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        public ExpenseController(IExpenseService expenseService) { 
            _expenseService = expenseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseRequest request)
        {
            var result = await _expenseService.AddExpenseAsync(request, UserId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserExpenses([FromQuery] int? month, [FromQuery] int? categoryId)
        {
            var result = await _expenseService.GetUserExpenses(UserId, month, categoryId);

            return Ok(result);
        }

        [HttpPatch("{expenseId}")]
        public async Task<IActionResult> UpdateExpense(int expenseId, [FromBody]ExpenseRequest request)
        {
            var result = await _expenseService.UpdateExpenseAsync(UserId, expenseId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);

        }

        [HttpDelete("{expenseId}")]
        public async Task<IActionResult> DeleteExpense(int expenseId)
        {
            var result = await _expenseService.DeleteExpenseAsync(UserId, expenseId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{expenseId}")]
        public async Task<IActionResult> GetExpenseById(int expenseId)
        {
            var result = await _expenseService.GetExpenseByIdAsync(UserId, expenseId);

            return Ok(result);
        }

    }
}
