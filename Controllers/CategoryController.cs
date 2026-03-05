using HomeBudgetAPI.DTOs.Category;
using HomeBudgetAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeBudgetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
        {
            var result = await _categoryService.AddCategoryAsync(request, UserId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserCategories()
        {
            var categories = await _categoryService.GetUserCategories(UserId);

            return Ok(categories);
        }

        [HttpPatch("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryRequest request)
        {
            var result = await _categoryService.UpdateCategoryAsync(UserId, categoryId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId, [FromQuery] int? moveToCategoryId)
        {
            var result = await _categoryService.DeleteCategoryAsync(UserId, categoryId, moveToCategoryId);

            if (!result.Success)
            {

                if (result.Message.Contains("ne pripada", StringComparison.OrdinalIgnoreCase))
                    return Forbid();
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var result = await _categoryService.GetCategoryByIdAsync(UserId, categoryId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }


    }
}
