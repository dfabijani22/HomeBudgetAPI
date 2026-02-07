using HomeBudgetAPI.DTOs;
using HomeBudgetAPI.Models;

namespace HomeBudgetAPI.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse> AddCategoryAsync(CategoryRequest request, int UserId);
        Task<List<CategoryDTO>> GetUserCategories(int userId);

    }
}
