using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Repository;
using backend.DTOs.MenuItemDTO;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")] // Tự động thành /api/v1/menuitem
    public class MenuItemController : ControllerBase
    {
        private readonly MenuItemRepository _menuItemRepo;

        public MenuItemController(MenuItemRepository menuItemRepo)
        {
            _menuItemRepo = menuItemRepo;
        }

        // [MỚI THÊM] - API: GET /api/v1/menuitem (Dành cho Admin xem tất cả)
        [HttpGet]
        public async Task<IActionResult> GetAllMenuItems()
        {
            var menuItems = await _menuItemRepo.GetAllMenuItemsAsync();
            return Ok(menuItems);
        }

        // API: GET /api/v1/menuitem/eatery/{eateryId} (Dành cho App xem theo quán)
        [HttpGet("eatery/{eateryId}")]
        public async Task<IActionResult> GetMenuByEateryId(int eateryId)
        {
            var menuItems = await _menuItemRepo.GetMenuItemsByEateryIdAsync(eateryId);
            return Ok(menuItems);
        }

        // API: POST /api/v1/menuitem (Sửa lại để nhận EateryId từ Body JSON của React)
        [HttpPost]
        public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemRequestDto request)
        {
            var newId = await _menuItemRepo.CreateMenuItemAsync(request);
            return Ok(new { message = "Thêm món ăn thành công!", id = newId });
        }

        // API: PUT /api/v1/menuitem/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] UpdateMenuItemRequestDto request)
        {
            var success = await _menuItemRepo.UpdateMenuItemAsync(id, request);
            if (!success) return NotFound(new { message = "Không tìm thấy món ăn này!" });
            
            return Ok(new { message = "Cập nhật món ăn thành công!" });
        }

        // API: DELETE /api/v1/menuitem/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var success = await _menuItemRepo.DeleteMenuItemAsync(id);
            if (!success) return NotFound(new { message = "Không tìm thấy món ăn để xóa!" });
            
            return Ok(new { message = "Đã xóa món ăn khỏi thực đơn!" });
        }
    }
}