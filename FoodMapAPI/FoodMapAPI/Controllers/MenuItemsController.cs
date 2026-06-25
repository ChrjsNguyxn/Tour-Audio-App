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

        // GET: api/menuitems/eatery/5
        [HttpGet("eatery/{eateryId}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetByEatery(int eateryId)
        {
            return Ok(await _menuRepo.GetByEateryIdAsync(eateryId));
        }

        // POST: api/menuitems
        [HttpPost]
        public async Task<ActionResult<MenuItem>> Create(MenuItemDto dto)
        {
            var item = new MenuItem
            {
                Name = dto.Name,
                ImagePath = dto.ImagePath,
                Price = dto.Price,
                Description = dto.Description,
                IsAvailable = dto.IsAvailable,
                EateryId = dto.EateryId
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
                Price = dto.Price,
                Description = dto.Description,
                IsAvailable = dto.IsAvailable
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