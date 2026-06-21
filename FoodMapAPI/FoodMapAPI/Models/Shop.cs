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
    }
}