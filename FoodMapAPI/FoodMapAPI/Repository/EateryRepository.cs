using Microsoft.EntityFrameworkCore;
using FoodMapAPI.Data;
using FoodMapAPI.Models;

namespace FoodMapAPI.Repository
{
    public class EateryRepository : IEateryRepository
    {
        private readonly AppDbContext _context;
        public EateryRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<Eatery>> GetAllAsync()
        {
            return await _context.Eateries.ToListAsync();
        }

        public async Task<Eatery?> GetByIdAsync(int id)
        {
            return await _context.Eateries.FindAsync(id);
        }

        public async Task<IEnumerable<Eatery>> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.Eateries.Where(e => e.OwnerId == ownerId).ToListAsync();
        }

        public async Task<Eatery> CreateAsync(Eatery eatery)
        {
            _context.Eateries.Add(eatery);
            await _context.SaveChangesAsync();
            return eatery;
        }

        public async Task<bool> UpdateAsync(int id, Eatery eatery)
        {
            var existing = await _context.Eateries.FindAsync(id);
            if (existing == null) return false;

            existing.Name = eatery.Name;
            existing.Address = eatery.Address;
            existing.CategoryId = eatery.CategoryId;
            existing.PriceRange = eatery.PriceRange;
            existing.Description = eatery.Description;
            existing.Latitude = eatery.Latitude;
            existing.Longitude = eatery.Longitude;
            existing.AudioFilePath = eatery.AudioFilePath;
            existing.ImagePath = eatery.ImagePath;
            existing.OpenTime = eatery.OpenTime;
            existing.CloseTime = eatery.CloseTime;
            existing.IsOpenNow = eatery.IsOpenNow;
            existing.NarrationText = eatery.NarrationText;
            existing.NarrationLanguage = eatery.NarrationLanguage;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var eatery = await _context.Eateries.FindAsync(id);
            if (eatery == null) return false;
            _context.Eateries.Remove(eatery);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}