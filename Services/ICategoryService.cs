using HomeBudgetAPI.DTOs;
using HomeBudgetAPI.DTOs.Category;
using HomeBudgetAPI.DTOs.Common;

namespace HomeBudgetAPI.Services
{
    public interface ICategoryService
    {
        Task<ApiResponse<CategoryResponse>> AddCategoryAsync(CategoryRequest request, int UserId);
        Task<ApiResponse<List<CategoryResponse>>> GetUserCategories(int userId);
        Task<ApiResponse<CategoryResponse>> UpdateCategoryAsync(int userId, int categoryId, CategoryRequest request);
        Task<ApiResponse<CategoryResponse>> DeleteCategoryAsync(int userId, int categoryId, int? moveToCategoryId);
        Task<ApiResponse<CategoryResponse>> GetCategoryByIdAsync(int userId, int categoryId);

    }
}
