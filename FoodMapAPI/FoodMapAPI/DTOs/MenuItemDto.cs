namespace FoodMapAPI.DTOs
{
    public class MenuItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Price { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ShopId { get; set; }
    }
}