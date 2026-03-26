using HomeBudgetAPI.DTOs.Common;
using HomeBudgetAPI.DTOs.MonthlyBudget;

namespace HomeBudgetAPI.Services
{

    public interface IMonthlyBudgetService
    {
        Task<ApiResponse<MonthlyBudgetResponse?>> GetBudgetAsync(int year, int month, int userId);
        Task<ApiResponse<MonthlyBudgetResponse>> SetBudgetAsync(MonthlyBudgetRequest request, int userId);
    }
}
