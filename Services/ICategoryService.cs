using HomeBudgetAPI.Models;

namespace HomeBudgetAPI.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse> AddCategoryAsync(CategoryRequest request, int UserId);
    }
}
