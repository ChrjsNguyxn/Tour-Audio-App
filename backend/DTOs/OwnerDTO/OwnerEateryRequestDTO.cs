using backend.DTOs.EateryDTO;

namespace backend.DTOs.EateryDTO
{
    // Kế thừa CreateEateryRequestDto đã có (Name, Address, CategoryId, PriceRange,
    // Description, Latitude, Longitude, AudioFilePath, ImagePath, OpenTime, CloseTime)
    // và CHỈ thêm các field mà Dashboard.jsx cần mà DTO gốc chưa có.
    // Không sửa file EateryResponseDTO.cs gốc.
    public class OwnerCreateEateryRequestDto : CreateEateryRequestDto
    {
        public int OwnerId { get; set; }
        public bool IsOpenNow { get; set; } = true;
        public string? NarrationText { get; set; }
        public string? NarrationLanguage { get; set; } = "vi-VN";
    }
}
