using HomeBudgetAPI.Data;
using HomeBudgetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CategoryResponse> AddCategoryAsync(CategoryRequest categoryRequest, int userId)
        {
            var existingCategory = await _context.Categories.AnyAsync(c => c.UserId == userId && c.Name.ToLower() == categoryRequest.Name.ToLower());
            if (existingCategory == true)
            {
                return new CategoryResponse { Success = false, Message = "Postoji kategorija pod tim imenom." };
            }

            var category = new Category
            {
                Name = categoryRequest.Name,
                Description = categoryRequest.Description,
                UserId = userId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryResponse { Success = true, Message = "Kategorija uspješno dodana.", CategoryId = category.Id };

        }
    }
}
