using Microsoft.EntityFrameworkCore;
using FoodMapAPI.Data;
using FoodMapAPI.Models;

namespace FoodMapAPI.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly AppDbContext _context;
        public OwnerRepository(AppDbContext context) { _context = context; }

        public async Task<Owner?> GetByIdAsync(int id)
        {
            return await _context.Owners
                .Include(o => o.Shops)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Owner?> GetByEmailAsync(string email)
        {
            return await _context.Owners.FirstOrDefaultAsync(o => o.Email == email);
        }

        public async Task<Owner> CreateAsync(Owner owner)
        {
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
            return owner;
        }
    }
}