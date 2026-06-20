using FoodMapAPI.Models;

namespace FoodMapAPI.Repository
{
    public interface IShopRepository
    {
        Task<IEnumerable<Shop>> GetAllAsync();
        Task<Shop?> GetByIdAsync(int id);
        Task<IEnumerable<Shop>> GetByOwnerIdAsync(int ownerId);
        Task<Shop> CreateAsync(Shop shop);
        Task<bool> UpdateAsync(int id, Shop shop);
        Task<bool> DeleteAsync(int id);
    }
}