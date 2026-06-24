using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using backend.Repository;
using backend.DTOs.MenuItemDTO;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly MenuItemRepository _menuItemRepo;

        public MenuItemController(MenuItemRepository menuItemRepo)
        {
            _menuItemRepo = menuItemRepo;
        }

        // API: GET /api/v1/menuitem/eatery/{eateryId}
        [HttpGet("eatery/{eateryId}")]
        public async Task<IActionResult> GetMenuByEateryId(int eateryId)
        {
            var menuItems = await _menuItemRepo.GetMenuItemsByEateryIdAsync(eateryId);
            return Ok(menuItems);
        }

        // Lấy ra menu đang được phục vụ cho tourist xem
        // API: GET /api/v1/menuitem/eatery/{eateryId}/menu
        [HttpGet("eateries/{eateryId}/menu")]
        public async Task<IActionResult> GetAvailableMenu(int eateryId)
        {
            var menu = await _menuItemRepo.GetAvailableMenuItemsByEateryIdAsync(eateryId);

            return Ok(menu);
        }

        // API: POST /api/v1/menuitem/eatery/{eateryId}
        [HttpPost("eatery/{eateryId}")]
        public async Task<IActionResult> CreateMenuItem(int eateryId, [FromBody] CreateMenuItemRequestDto request)
        {
            var newId = await _menuItemRepo.CreateMenuItemAsync(eateryId, request);
            return CreatedAtAction(nameof(GetMenuByEateryId), new { eateryId = eateryId }, new { message = "Thêm món ăn thành công!", id = newId });
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