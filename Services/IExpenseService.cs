using HomeBudgetAPI.DTOs.Common;
using HomeBudgetAPI.DTOs.Expense;

namespace HomeBudgetAPI.Services
{
    public interface IExpenseService
    {
        Task<ApiResponse<int>> AddExpenseAsync(ExpenseRequest request, int UserId);
        Task<ApiResponse<List<ExpenseResponse>>> GetUserExpenses(int userId, int? month = null, int? categoryId = null);
        Task<ApiResponse<int>> UpdateExpenseAsync(int userId, int expenseId, ExpenseRequest request);
        Task<ApiResponse<int>> DeleteExpenseAsync(int userId, int expenseId);
        Task<ApiResponse<ExpenseResponse>> GetExpenseByIdAsync(int userId, int expenseId);
    }
}
