using FoodMapAPI.Models;

namespace FoodMapAPI.Repository
{
    public interface IEateryRepository
    {
        Task<IEnumerable<Eatery>> GetAllAsync();
        Task<Eatery?> GetByIdAsync(int id);
        Task<IEnumerable<Eatery>> GetByOwnerIdAsync(int ownerId);
        Task<Eatery> CreateAsync(Eatery eatery);
        Task<bool> UpdateAsync(int id, Eatery eatery);
        Task<bool> DeleteAsync(int id);
    }
}