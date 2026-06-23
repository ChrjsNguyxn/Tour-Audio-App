namespace backend.DTOs.TouristDTO
{
    // Khuôn mẫu hiển thị quán ăn cho Khách (ẩn đi các thông tin nhạy cảm như OwnerId)
    public class TouristEateryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? PriceRange { get; set; }
        public string? Description { get; set; }
        public double Rating { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? AudioFilePath { get; set; } // Link audio thuyết minh
        public string? ImagePath { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
    }
}