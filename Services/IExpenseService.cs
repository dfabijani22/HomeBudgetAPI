using HomeBudgetAPI.DTOs;
using HomeBudgetAPI.Models;

namespace HomeBudgetAPI.Services
{
    public interface IExpenseService
    {
        Task<ExpenseResponse> AddExpenseAsync(ExpenseRequest request, int UserId);
        Task<List<ExpenseDTO>> GetUserExpenses(int userId, int? month = null, int? categoryId = null);
        Task<ExpenseResponse> UpdateExpenseAsync(int userId, int expenseId, ExpenseRequest request);
        Task<ExpenseResponse> DeleteExpenseAsync(int userId, int expenseId);
        Task<ExpenseDTO> GetExpenseByIdAsync(int userId, int expenseId);
    }
}
