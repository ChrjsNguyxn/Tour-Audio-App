namespace FoodMapAPI.DTOs
{
    public class EateryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string PriceRange { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AudioFilePath { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string OpenTime { get; set; } = "06:00";
        public string CloseTime { get; set; } = "22:00";
        public bool IsOpenNow { get; set; } = true;
        public string NarrationText { get; set; } = string.Empty;
        public string NarrationLanguage { get; set; } = "vi-VN";
        public int OwnerId { get; set; }
    }
}