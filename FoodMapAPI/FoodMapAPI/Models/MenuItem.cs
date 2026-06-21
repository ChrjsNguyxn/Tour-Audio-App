namespace FoodMapAPI.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Price { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Liên kết với quán
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}