using FoodMapAPI.Models;

namespace FoodMapAPI.Repository
{
    public interface IOwnerRepository
    {
        Task<Owner?> GetByIdAsync(int id);
        Task<Owner?> GetByEmailAsync(string email);
        Task<Owner> CreateAsync(Owner owner);
    }
}