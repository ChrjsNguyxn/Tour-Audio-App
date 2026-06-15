using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodMapAPI.Data;
using FoodMapAPI.Models;

namespace FoodMapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShopsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/shops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> GetShops()
        {
            return await _context.Shops.ToListAsync();
        }

        // GET: api/shops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> GetShop(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null) return NotFound();
            return shop;
        }

        // POST: api/shops
        [HttpPost]
        public async Task<ActionResult<Shop>> CreateShop(Shop shop)
        {
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetShop), new { id = shop.Id }, shop);
        }

        // PUT: api/shops/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(int id, Shop shop)
        {
            if (id != shop.Id) return BadRequest();
            _context.Entry(shop).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/shops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null) return NotFound();
            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}