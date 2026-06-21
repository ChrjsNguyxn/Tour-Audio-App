using Microsoft.AspNetCore.Mvc;
using FoodMapAPI.DTOs;
using FoodMapAPI.Models;
using FoodMapAPI.Repository;

namespace FoodMapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemRepository _menuRepo;

        public MenuItemsController(IMenuItemRepository menuRepo)
        {
            _menuRepo = menuRepo;
        }

        // GET: api/menuitems/shop/5
        [HttpGet("shop/{shopId}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetByShop(int shopId)
        {
            return Ok(await _menuRepo.GetByShopIdAsync(shopId));
        }

        // POST: api/menuitems
        [HttpPost]
        public async Task<ActionResult<MenuItem>> Create(MenuItemDto dto)
        {
            var item = new MenuItem
            {
                Name = dto.Name,
                ImagePath = dto.ImagePath,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Description = dto.Description,
                ShopId = dto.ShopId
            };

            var created = await _menuRepo.CreateAsync(item);
            return Ok(created);
        }

        // PUT: api/menuitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MenuItemDto dto)
        {
            var item = new MenuItem
            {
                Name = dto.Name,
                ImagePath = dto.ImagePath,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Description = dto.Description
            };

            var success = await _menuRepo.UpdateAsync(id, item);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE: api/menuitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _menuRepo.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}