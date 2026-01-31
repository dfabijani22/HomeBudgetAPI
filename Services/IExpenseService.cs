using HomeBudgetAPI.Models;

namespace HomeBudgetAPI.Services
{
    public interface IExpenseService
    {
        Task<ExpenseResponse> AddExpenseAsync(ExpenseRequest request, int UserId);
    }
}
