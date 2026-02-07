using HomeBudgetAPI.Data;
using HomeBudgetAPI.DTOs;
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

        public async Task<List<CategoryDTO>> GetUserCategories(int userId)
        {
            var query = _context.Categories
                .Where(e => e.UserId == userId || e.IsDefault == true);

            return await query
                .Select(e => new CategoryDTO
                {
                    Name = e.Name,
                    Description= e.Description,
                    IsDefault = e.IsDefault,
                })
                .ToListAsync();
        }
    }
}
