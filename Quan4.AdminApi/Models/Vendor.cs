namespace Quan4.AdminApi.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        
        public string PriceRange { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AudioFilePath { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string OpenTime { get; set; } = string.Empty;
        public string CloseTime { get; set; } = string.Empty;
        
        public int OwnerId { get; set; }
        public User? Owner { get; set; }
        
        public bool IsApproved { get; set; } = false; // Quyền duyệt của Admin
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}