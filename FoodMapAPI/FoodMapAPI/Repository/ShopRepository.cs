using Microsoft.EntityFrameworkCore;
using FoodMapAPI.Data;
using FoodMapAPI.Models;

namespace FoodMapAPI.Repository
{
    public class ShopRepository : IShopRepository
    {
        private readonly AppDbContext _context;
        public ShopRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<Shop>> GetAllAsync()
        {
            return await _context.Shops.ToListAsync();
        }

        public async Task<Shop?> GetByIdAsync(int id)
        {
            return await _context.Shops.FindAsync(id);
        }

        public async Task<IEnumerable<Shop>> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.Shops.Where(s => s.OwnerId == ownerId).ToListAsync();
        }

        public async Task<Shop> CreateAsync(Shop shop)
        {
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();
            return shop;
        }

        public async Task<bool> UpdateAsync(int id, Shop shop)
        {
            var existing = await _context.Shops.FindAsync(id);
            if (existing == null) return false;

            existing.Name = shop.Name;
            existing.Category = shop.Category;
            existing.PriceRange = shop.PriceRange;
            existing.Description = shop.Description;
            existing.Latitude = shop.Latitude;
            existing.Longitude = shop.Longitude;
            existing.AudioFilePath = shop.AudioFilePath;
            existing.ImagePath = shop.ImagePath;
            existing.OpenTime = shop.OpenTime;
            existing.CloseTime = shop.CloseTime;
            existing.IsOpenNow = shop.IsOpenNow;
            existing.NarrationText = shop.NarrationText;
            existing.NarrationLanguage = shop.NarrationLanguage;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null) return false;
            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}