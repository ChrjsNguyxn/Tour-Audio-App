using Microsoft.AspNetCore.Mvc;
using backend.Repository;
using Backend.DTOs.CategoryDTO;       // Đã đổi thành b thường

namespace backend.Controllers   // Đã đổi thành b thường
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepo;

        public CategoryController(CategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // API: GET /api/v1/category
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepo.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // API: POST /api/v1/category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            var newId = await _categoryRepo.CreateCategoryAsync(request);
            return CreatedAtAction(nameof(GetAllCategories), new { id = newId }, new { message = "Tạo danh mục thành công!", id = newId });
        }

        // API: PUT /api/v1/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequestDto request)
        {
            var success = await _categoryRepo.UpdateCategoryAsync(id, request);
            if (!success)
            {
                return NotFound(new { message = "Không tìm thấy danh mục để cập nhật!" });
            }
            return Ok(new { message = "Cập nhật danh mục thành công!" });
        }
    }
}