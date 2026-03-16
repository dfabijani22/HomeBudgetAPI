using HomeBudgetAPI.DTOs.Common;
using HomeBudgetAPI.DTOs.Expense;

namespace HomeBudgetAPI.Services
{
    public interface IExpenseService
    {
        Task<ApiResponse<ExpenseResponse>> AddExpenseAsync(ExpenseRequest request, int UserId);
        Task<ApiResponse<List<ExpenseResponse>>> GetUserExpenses(int userId, int? month = null, int? categoryId = null);
        Task<ApiResponse<ExpenseResponse>> UpdateExpenseAsync(int userId, int expenseId, ExpenseRequest request);
        Task<ApiResponse<ExpenseResponse>> DeleteExpenseAsync(int userId, int expenseId);
        Task<ApiResponse<ExpenseResponse>> GetExpenseByIdAsync(int userId, int expenseId);
    }
}
