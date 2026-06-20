using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quan4.AdminApi.Data;
using Quan4.AdminApi.DTOs;
using Quan4.AdminApi.Models;

namespace Quan4.AdminApi.Controllers
{
    [Route("api/v1/admin/categories")]
    [ApiController]
    [Authorize] // Ổ khóa: Chỉ có người có Token mới được quản lý danh mục
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LẤY DANH SÁCH DANH MỤC (GET)
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();

            return Ok(categories);
        }

        // 2. TẠO DANH MỤC MỚI (POST)
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            // Kiểm tra xem tên danh mục đã tồn tại chưa (tránh tạo trùng)
            if (await _context.Categories.AnyAsync(c => c.Name == request.Name))
            {
                return BadRequest("Danh mục này đã tồn tại!");
            }

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã tạo danh mục '{request.Name}' thành công!" });
        }
    }
}