using backend.DTOs;
using backend.Repository;

namespace backend.Services;

public class EateryService
{
    private readonly EateryRepository _eateryRepository;
    private readonly CategoryRepository _categoryRepository;

    public EateryService(
        EateryRepository eateryRepository,
        CategoryRepository categoryRepository)
    {
        _eateryRepository = eateryRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<EateryResponseDto>> GetAllAsync()
    {
        var eateries = await _eateryRepository.GetAllAsync();

        return eateries.Select(e => new EateryResponseDto
        {
            Id = e.Id,
            Name = e.Name,
            Address = e.Address,

            OwnerId = e.OwnerId,
            CategoryId = e.CategoryId,

            PriceRange = e.PriceRange,
            Description = e.Description,

            Rating = e.Rating,

            Latitude = e.Latitude,
            Longitude = e.Longitude,

            AudioFilePath = e.AudioFilePath,
            ImagePath = e.ImagePath,

            OpenTime = e.OpenTime,
            CloseTime = e.CloseTime,

            IsApproved = e.IsApproved
        });
    }

    public async Task<IEnumerable<EateryResponseDto>> GetAllWithCategoryNameAsync()
    {
        var eateries = await _eateryRepository.GetAllAsync();

        var categories =
            await _categoryRepository.GetAllAsync();

        var categoryMap = categories.ToDictionary(
            c => c.Id,
            c => c.Name
        );

        return eateries.Select(e => new EateryResponseDto
        {
            Id = e.Id,
            Name = e.Name,
            Address = e.Address,

            OwnerId = e.OwnerId,

            CategoryId = e.CategoryId,

            CategoryName =
                categoryMap.GetValueOrDefault(
                    e.CategoryId,
                    "Unknown"
                ),

            PriceRange = e.PriceRange,
            Description = e.Description,

            Rating = e.Rating,

            Latitude = e.Latitude,
            Longitude = e.Longitude,

            AudioFilePath = e.AudioFilePath,
            ImagePath = e.ImagePath,

            OpenTime = e.OpenTime,
            CloseTime = e.CloseTime,

            IsApproved = e.IsApproved
        });
    }
}