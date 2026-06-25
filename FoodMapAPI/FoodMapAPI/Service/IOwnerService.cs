using FoodMapAPI.DTOs;

namespace FoodMapAPI.Service
{
    public interface IOwnerService
    {
        Task<OwnerResponseDto> RegisterAsync(RegisterOwnerDto dto);
        Task<object> LoginAsync(LoginOwnerDto dto);
        Task<OwnerResponseDto?> GetByIdAsync(int id);
    }
}