using Microsoft.AspNetCore.Mvc;
using FoodMapAPI.DTOs;
using FoodMapAPI.Models;
using FoodMapAPI.Repository;

namespace FoodMapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopsController : ControllerBase
    {
        private readonly IShopRepository _shopRepo;

        public ShopsController(IShopRepository shopRepo)
        {
            _shopRepo = shopRepo;
        }

        // GET: api/shops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> GetShops()
        {
            return Ok(await _shopRepo.GetAllAsync());
        }

        // GET: api/shops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> GetShop(int id)
        {
            var shop = await _shopRepo.GetByIdAsync(id);
            if (shop == null) return NotFound();
            return Ok(shop);
        }

        // GET: api/shops/owner/5
        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IEnumerable<Shop>>> GetByOwner(int ownerId)
        {
            return Ok(await _shopRepo.GetByOwnerIdAsync(ownerId));
        }

        // POST: api/shops
        [HttpPost]
        public async Task<ActionResult<Shop>> CreateShop(ShopDto dto)
        {
            var shop = new Shop
            {
                Name = dto.Name,
                Category = dto.Category,
                PriceRange = dto.PriceRange,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                AudioFilePath = dto.AudioFilePath,
                ImagePath = dto.ImagePath,
                OpenTime = dto.OpenTime,
                CloseTime = dto.CloseTime,
                IsOpenNow = dto.IsOpenNow,
                NarrationText = dto.NarrationText,
                NarrationLanguage = dto.NarrationLanguage,
                OwnerId = dto.OwnerId
            };

            var created = await _shopRepo.CreateAsync(shop);
            return CreatedAtAction(nameof(GetShop), new { id = created.Id }, created);
        }

        // PUT: api/shops/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(int id, ShopDto dto)
        {
            var shop = new Shop
            {
                Name = dto.Name,
                Category = dto.Category,
                PriceRange = dto.PriceRange,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                AudioFilePath = dto.AudioFilePath,
                ImagePath = dto.ImagePath,
                OpenTime = dto.OpenTime,
                CloseTime = dto.CloseTime,
                IsOpenNow = dto.IsOpenNow,
                NarrationText = dto.NarrationText,
                NarrationLanguage = dto.NarrationLanguage
            };

            var success = await _shopRepo.UpdateAsync(id, shop);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE: api/shops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var success = await _shopRepo.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}