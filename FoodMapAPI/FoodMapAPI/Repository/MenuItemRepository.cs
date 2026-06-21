using Microsoft.EntityFrameworkCore;
using FoodMapAPI.Data;
using FoodMapAPI.Models;

namespace FoodMapAPI.Repository
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDbContext _context;
        public MenuItemRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<MenuItem>> GetByShopIdAsync(int shopId)
        {
            return await _context.MenuItems.Where(m => m.ShopId == shopId).ToListAsync();
        }

        public async Task<MenuItem> CreateAsync(MenuItem item)
        {
            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> UpdateAsync(int id, MenuItem item)
        {
            var existing = await _context.MenuItems.FindAsync(id);
            if (existing == null) return false;

            existing.Name = item.Name;
            existing.ImagePath = item.ImagePath;
            existing.Quantity = item.Quantity;
            existing.Price = item.Price;
            existing.Description = item.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return false;
            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}