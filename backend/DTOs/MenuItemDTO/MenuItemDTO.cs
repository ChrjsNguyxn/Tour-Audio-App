namespace backend.DTOs.MenuItemDTO
{
    public class MenuItemResponseDto
    {
        public int Id { get; set; }
        public int EateryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public string? AudioPath { get; set; } // Thêm Audio cho tính năng nghe thuyết minh
        public bool IsAvailable { get; set; }
        public string? CreatedAt { get; set; }
    }

    public class CreateMenuItemRequestDto
    {
        public int EateryId { get; set; } // Admin Frontend sẽ gửi EateryId vào đây
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public string? AudioPath { get; set; } 
        public bool IsAvailable { get; set; } = true; 
    }

    public class UpdateMenuItemRequestDto
    {
        public int EateryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public string? AudioPath { get; set; }
        public bool IsAvailable { get; set; } = true; // Sửa lại mặc định bằng true
    }
}