using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quan4.AdminApi.Models
{
    [Table("vendors")]
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        // Đã thêm trường Address để giải quyết lỗi báo đỏ
        [Required]
        public string Address { get; set; } = string.Empty;
        
        public int CategoryId { get; set; }
        
        public string? PriceRange { get; set; }
        
        public string? Description { get; set; }
        
        // Đã thêm trường Rating theo đúng biểu quyết Database MVP
        public double Rating { get; set; } = 0.0;
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
        
        public string? AudioFilePath { get; set; }
        
        public string? ImagePath { get; set; }
        
        public string? OpenTime { get; set; }
        
        public string? CloseTime { get; set; }
        
        public int OwnerId { get; set; }
        
        public bool IsApproved { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ==========================================
        // CÁC KHÓA NGOẠI (NAVIGATION PROPERTIES)
        // ==========================================
        
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        
        [ForeignKey("OwnerId")]
        public User? Owner { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}