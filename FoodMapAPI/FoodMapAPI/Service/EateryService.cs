using FoodMapAPI.DTOs;
using FoodMapAPI.Models;
using FoodMapAPI.Repository;

namespace FoodMapAPI.Service
{
    public class EateryService : IEateryService
    {
        private readonly IEateryRepository _eateryRepo;

        public EateryService(IEateryRepository eateryRepo)
        {
            _eateryRepo = eateryRepo;
        }

        public Task<IEnumerable<Eatery>> GetAllAsync() => _eateryRepo.GetAllAsync();

        public Task<Eatery?> GetByIdAsync(int id) => _eateryRepo.GetByIdAsync(id);

        public Task<IEnumerable<Eatery>> GetByOwnerIdAsync(int ownerId) => _eateryRepo.GetByOwnerIdAsync(ownerId);

        public async Task<Eatery> CreateAsync(EateryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Tên quán không được để trống.");
            if (string.IsNullOrWhiteSpace(dto.Address))
                throw new ArgumentException("Địa chỉ không được để trống.");

            var eatery = MapToModel(dto);
            return await _eateryRepo.CreateAsync(eatery);
        }

        public async Task<bool> UpdateAsync(int id, EateryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Tên quán không được để trống.");

            var eatery = MapToModel(dto);
            return await _eateryRepo.UpdateAsync(id, eatery);
        }

        public Task<bool> DeleteAsync(int id) => _eateryRepo.DeleteAsync(id);

        private static Eatery MapToModel(EateryDto dto)
        {
            return new Eatery
            {
                Name = dto.Name,
                Address = dto.Address,
                CategoryId = dto.CategoryId,
                PriceRange = dto.PriceRange,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                AudioFilePath = dto.AudioFilePath,
                ImagePath = dto.ImagePath,
                OpenTime = dto.OpenTime,
                CloseTime = dto.CloseTime,
                IsOpenNow = dto.IsOpenNow,
                NarrationText = dto.NarrationText,
                NarrationLanguage = dto.NarrationLanguage,
                OwnerId = dto.OwnerId
            };
        }
    }
}