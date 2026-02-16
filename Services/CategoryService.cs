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
                    Description = e.Description,
                    IsDefault = e.IsDefault,
                })
                .ToListAsync();
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(int userId, int categoryId, CategoryRequest request)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

            if (category == null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Message = "Kategorija nije pronađena ili ne pripada korisniku."
                };
            }

            if (category.IsDefault)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Message = "Default kategorije nije moguće uređivati."
                };
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
                category.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.Description))
                category.Description = request.Description;

            await _context.SaveChangesAsync();

            return new CategoryResponse
            {
                Success = true,
                Message = "Kategorija uspješno ažurirana.",
                CategoryId = category.Id
            };
        }

        public async Task<CategoryResponse> DeleteCategoryAsync(int userId, int categoryId, int? moveToCategoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                return new CategoryResponse { Success = false, Message = "Kategorija ne postoji." };
            }

            if (category.IsDefault && category.UserId == null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Message = "Globalne default kategorije nije moguće brisati."
                };
            }

            if (category.UserId != userId)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Message = "Kategorija ne pripada korisniku."
                };
            }

            Category targetCategory;

            if (moveToCategoryId.HasValue)
            {
                targetCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == moveToCategoryId && (c.UserId == userId || c.UserId == null));

                if (targetCategory == null)
                {
                    return new CategoryResponse
                    {
                        Success = false,
                        Message = "Kategorija za prebacivanje troškova ne postoji."
                    };
                }
            }
            else
            {
                targetCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.IsDefault && c.UserId == null);

                if (targetCategory == null)
                {
                    return new CategoryResponse
                    {
                        Success = false,
                        Message = "Fallback default kategorija nije pronađena."
                    };
                }
            }

            var expensesToMove = await _context.Expenses
                .Where(e => e.CategoryId == categoryId && e.UserId == userId)
                .ToListAsync();

            foreach (var expense in expensesToMove)
                expense.CategoryId = targetCategory.Id;

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return new CategoryResponse
            {
                Success = true,
                Message = $"Kategorija obrisana. {expensesToMove.Count} troškova prebačeno u '{targetCategory.Name}'.",
                CategoryId = category.Id
            };
        }

    }
}
