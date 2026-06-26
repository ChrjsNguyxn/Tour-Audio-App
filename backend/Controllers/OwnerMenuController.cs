using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Repository;
using backend.DTOs.MenuItemDTO;

namespace backend.Controllers
{

    [ApiController]
    [Route("api/v1/owner/menu")]
    public class OwnerMenuController : ControllerBase
    {
        private readonly MenuItemRepository _menuItemRepo;

        public OwnerMenuController(MenuItemRepository menuItemRepo)
        {
            _menuItemRepo = menuItemRepo;
        }

        // API: GET /api/v1/owner/menu/eatery/{eateryId}
        [HttpGet("eatery/{eateryId}")]
        public async Task<IActionResult> GetMenuByEatery(int eateryId)
        {
            var items = await _menuItemRepo.GetMenuItemsByEateryIdAsync(eateryId);
            return Ok(items);
        }

        // API: POST /api/v1/owner/menu
        // Body: { name, price, description, imagePath, isAvailable, eateryId }
        public class CreateOwnerMenuItemRequestDto : CreateMenuItemRequestDto
        {
            public int EateryId { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuItem([FromBody] CreateOwnerMenuItemRequestDto request)
        {
            var newId = await _menuItemRepo.CreateMenuItemAsync(request.EateryId, request);
            return Ok(new { message = "Thêm món ăn thành công!", id = newId });
        }

        // API: DELETE /api/v1/owner/menu/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var success = await _menuItemRepo.DeleteMenuItemAsync(id);
            if (!success) return NotFound(new { message = "Không tìm thấy món ăn để xóa!" });
            return Ok(new { message = "Đã xóa món ăn." });
        }
    }
}
