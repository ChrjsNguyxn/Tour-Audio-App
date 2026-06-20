namespace Quan4.AdminApi.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        
        public int VendorId { get; set; }
        public Vendor? Vendor { get; set; }
        
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}