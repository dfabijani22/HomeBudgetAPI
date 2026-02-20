using HomeBudgetAPI.DTOs;
using HomeBudgetAPI.Models;

namespace HomeBudgetAPI.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse> AddCategoryAsync(CategoryRequest request, int UserId);
        Task<List<CategoryDTO>> GetUserCategories(int userId);
        Task<CategoryResponse> UpdateCategoryAsync(int userId, int categoryId, CategoryRequest request);
        Task<CategoryResponse> DeleteCategoryAsync(int userId, int categoryId, int? moveToCategoryId);
        Task<CategoryDTO> GetCategoryByIdAsync(int userId, int categoryId);

    }
}
