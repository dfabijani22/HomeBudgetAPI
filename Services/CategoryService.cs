using AutoMapper;
using HomeBudgetAPI.Data;
using HomeBudgetAPI.DTOs.Category;
using HomeBudgetAPI.DTOs.Common;
using HomeBudgetAPI.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace HomeBudgetAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private const string NameCollation = "SQL_Latin1_General_CP1_CI_AI";

        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResponse<CategoryResponse>> AddCategoryAsync(CategoryRequest categoryRequest, int userId)
        {
            var name = categoryRequest.Name?.Trim();
            var exists = await _context.Categories.AnyAsync(c => c.UserId == userId && EF.Functions.Collate(c.Name, NameCollation) == name);
            if (exists)
            {
                return new ApiResponse<CategoryResponse> { Success = false, Message = "Postoji kategorija pod tim imenom.", Data = null };
            }

            var category = _mapper.Map<Category>(categoryRequest);
            category.UserId = userId;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new ApiResponse<CategoryResponse> { Success = true, Message = "Kategorija uspješno dodana.", Data = _mapper.Map<CategoryResponse>(category)  };

        }

        public async Task<ApiResponse<List<CategoryResponse>>> GetUserCategories(int userId)
        {

            var categories = await _context.Categories
                            .Where(c => c.UserId == userId || c.IsDefault)
                            .OrderByDescending(c => c.IsDefault)
                            .ThenBy(c => c.Name)
                            .ProjectTo<CategoryResponse>(_mapper.ConfigurationProvider)
                            .ToListAsync();

            return new ApiResponse<List<CategoryResponse>>
            {
                Success = true,
                Message = "Kategorije uspješno dohvačene.",
                Data = categories
            };

        }

        public async Task<ApiResponse<CategoryResponse>> UpdateCategoryAsync(int userId, int categoryId, CategoryRequest request)
        {
            var newName = request.Name?.Trim();
            var newDescription = request.Description?.Trim();

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

            if (category == null)
            {
                return new ApiResponse<CategoryResponse>
                {
                    Success = false,
                    Message = "Kategorija nije pronađena ili ne pripada korisniku.",
                    Data = null
                };
            }

            if (category.IsDefault)
            {
                return new ApiResponse<CategoryResponse>
                {
                    Success = false,
                    Message = "Default kategorije nije moguće uređivati.",
                    Data = _mapper.Map<CategoryResponse>(category)
                };
            }

            if (!string.IsNullOrWhiteSpace(newName) && !string.Equals(newName, category.Name, StringComparison.Ordinal)){

                var exists = await _context.Categories
                        .AnyAsync(c =>
                            c.UserId == userId &&
                            c.Id != categoryId &&
                            EF.Functions.Collate(c.Name, NameCollation) == newName);

                if (exists)
                {
                    return new ApiResponse<CategoryResponse>
                    {
                        Success = false,
                        Message = "Kategorija s ovim nazivom već postoji.",
                        Data = null
                    };
                }

                category.Name = newName;
            }

            if (!string.IsNullOrWhiteSpace(newDescription))
                category.Description = newDescription;

            await _context.SaveChangesAsync();

            return new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = "Kategorija uspješno ažurirana.",
                Data= _mapper.Map<CategoryResponse>(category)
            };
        }

        public async Task<ApiResponse<CategoryResponse>> DeleteCategoryAsync(int userId, int categoryId, int? moveToCategoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                return new ApiResponse<CategoryResponse> { Success = false, Message = "Kategorija ne postoji.", Data = null };
            }

            if (category.IsDefault && category.UserId == null)
            {
                return new ApiResponse<CategoryResponse>
                {
                    Success = false,
                    Message = "Globalne default kategorije nije moguće brisati.",
                    Data = null
                };
            }

            if (category.UserId != userId)
            {
                return new ApiResponse<CategoryResponse>
                {
                    Success = false,
                    Message = "Kategorija ne pripada korisniku.",
                    Data = null
                };
            }

            Category targetCategory;

            if (moveToCategoryId.HasValue)
            {
                targetCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == moveToCategoryId && (c.UserId == userId || c.UserId == null));

                if (targetCategory == null)
                {
                    return new ApiResponse<CategoryResponse>
                    {
                        Success = false,
                        Message = "Kategorija za prebacivanje troškova ne postoji.",
                        Data = null
                    };
                }
            }
            else
            {
                targetCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.IsDefault && c.UserId == null);

                if (targetCategory == null)
                {
                    return new ApiResponse<CategoryResponse>
                    {
                        Success = false,
                        Message = "Fallback default kategorija nije pronađena.",
                        Data = null
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

            return new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = $"Kategorija obrisana. {expensesToMove.Count} troškova prebačeno u '{targetCategory.Name}'.",
                Data = _mapper.Map<CategoryResponse>(category)
            };
        }

        public async Task<ApiResponse<CategoryResponse>> GetCategoryByIdAsync(int userId, int categoryId)
        {

            var category = await _context.Categories
                            .Where(c => c.Id == categoryId && (c.UserId == userId || c.IsDefault))
                            .ProjectTo<CategoryResponse>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync();

            if (category == null)
            {
                return new ApiResponse<CategoryResponse>
                {
                    Success = false,
                    Message = "Kategorija nije pronađena.",
                    Data = null
                };

            }


            return new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = "Kategorija uspješno dohvaćena.",
                Data = category
            };


        }

    }
}
