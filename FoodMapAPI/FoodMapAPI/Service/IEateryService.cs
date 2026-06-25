using FoodMapAPI.DTOs;
using FoodMapAPI.Models;

namespace FoodMapAPI.Service
{
    public interface IEateryService
    {
        Task<IEnumerable<Eatery>> GetAllAsync();
        Task<Eatery?> GetByIdAsync(int id);
        Task<IEnumerable<Eatery>> GetByOwnerIdAsync(int ownerId);
        Task<Eatery> CreateAsync(EateryDto dto);
        Task<bool> UpdateAsync(int id, EateryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}