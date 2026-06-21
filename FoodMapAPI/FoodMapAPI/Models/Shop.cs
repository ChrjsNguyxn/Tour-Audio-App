namespace FoodMapAPI.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string PriceRange { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AudioFilePath { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public Owner? Owner { get; set; }
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public string OpenTime { get; set; } = "06:00";
        public string CloseTime { get; set; } = "22:00";
        public bool IsOpenNow { get; set; } = true;
        public string NarrationText { get; set; } = string.Empty; // Kịch bản thuyết minh dạng văn bản
        public string NarrationLanguage { get; set; } = "vi-VN"; // Ngôn ngữ: vi-VN, en-US, zh-CN
    }
}