namespace FoodMapAPI.Models
{
    public class Eatery
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public int CategoryId { get; set; }
        public string PriceRange { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Rating { get; set; } = 0.0;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AudioFilePath { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string OpenTime { get; set; } = "06:00";
        public string CloseTime { get; set; } = "22:00";
        public bool IsOpenNow { get; set; } = true;
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Giữ lại tính năng cộng thêm
        public string NarrationText { get; set; } = string.Empty;
        public string NarrationLanguage { get; set; } = "vi-VN";

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}