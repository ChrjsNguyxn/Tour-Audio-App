using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodMapAPI.Data;
using FoodMapAPI.Models;

namespace FoodMapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MenuItemsController(AppDbContext context) { _context = context; }

        // GET: api/menuitems/shop/5
        [HttpGet("shop/{shopId}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetByShop(int shopId)
        {
            return await _context.MenuItems
                .Where(m => m.ShopId == shopId)
                .ToListAsync();
        }

        // POST: api/menuitems
        [HttpPost]
        public async Task<ActionResult<MenuItem>> Create(MenuItem item)
        {
            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();
            return Ok(item);
        }

        // PUT: api/menuitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MenuItem item)
        {
            if (id != item.Id) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/menuitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return NotFound();
            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}