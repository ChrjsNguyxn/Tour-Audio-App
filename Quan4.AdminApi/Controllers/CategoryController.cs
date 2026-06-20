using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quan4.AdminApi.Data;

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

        // API LẤY DANH SÁCH DANH MỤC
        [HttpGet]
        [AllowAnonymous] // Ai cũng có thể xem danh mục để chọn
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories
                .Select(c => new 
                { 
                    id = c.Id, 
                    name = c.Name 
                })
                .ToListAsync();

            return Ok(categories);
        }
    }
}