namespace backend.DTOs.MenuItemDTO
{
    // 1. Khuôn mẫu dữ liệu trả về cho Frontend hiển thị
    public class MenuItemResponseDto
    {
        public int Id { get; set; }
        public int EateryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public bool IsAvailable { get; set; }
        public string? CreatedAt { get; set; }
    }

    // 2. Khuôn mẫu dữ liệu Chủ quán gửi lên khi Thêm món mới
    public class CreateMenuItemRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public bool IsAvailable { get; set; } = true; // Mặc định tạo xong là có bán
    }

    // 3. Khuôn mẫu dữ liệu Chủ quán gửi lên khi Sửa món
    public class UpdateMenuItemRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public bool IsAvailable { get; set; }
    }
}