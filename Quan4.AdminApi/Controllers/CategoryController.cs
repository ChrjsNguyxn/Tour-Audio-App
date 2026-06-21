using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quan4.AdminApi.Data;
using Quan4.AdminApi.Models;
using Quan4.AdminApi.DTOs;

namespace Quan4.AdminApi.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LẤY DANH SÁCH DANH MỤC (Code cũ)
        [HttpGet]
        [AllowAnonymous] 
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories
                .Select(c => new 
                { 
                    id = c.Id, 
                    name = c.Name,
                    description = c.Description
                })
                .ToListAsync();

            return Ok(categories);
        }

        // 2. TẠO DANH MỤC MỚI (Code mới thêm)
        [HttpPost]
        [AllowAnonymous] // Tạm thời mở cửa để bạn dễ test qua Scalar, mốt ghép Auth sau thì đổi thành [Authorize]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            // Kiểm tra xem có ai nhập tên trống không
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Tên danh mục không được để trống!");

            // Kiểm tra chống trùng lặp tên danh mục
            var exists = await _context.Categories.AnyAsync(c => c.Name.ToLower() == request.Name.ToLower());
            if (exists) 
                return BadRequest("Tên danh mục này đã tồn tại trên hệ thống!");

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Thêm danh mục '{category.Name}' thành công!", id = category.Id });
        }
    }
}