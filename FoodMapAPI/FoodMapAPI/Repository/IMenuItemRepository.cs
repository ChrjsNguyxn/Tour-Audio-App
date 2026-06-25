using FoodMapAPI.Models;

namespace FoodMapAPI.Repository
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItem>> GetByEateryIdAsync(int eateryId);
        Task<MenuItem> CreateAsync(MenuItem item);
        Task<bool> UpdateAsync(int id, MenuItem item);
        Task<bool> DeleteAsync(int id);
    }
}