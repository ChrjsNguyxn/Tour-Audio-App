using System;


namespace backend.DTOs.EateryDTO
{
    // ==========================================
    // 1. NHÓM REQUEST (Admin & Owner gửi lên)
    // ==========================================

    // DTO Tạo mới (Dùng chung cho Admin và Owner)
    public class CreateEateryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string? PriceRange { get; set; }
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? AudioFilePath { get; set; }
        public string? ImagePath { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
    }

    // DTO Cập nhật (Dùng chung cho Admin và Owner)
    public class UpdateEateryRequestDto : CreateEateryRequestDto
    {
        // Kế thừa toàn bộ từ Create vì lúc sửa cũng truyền lên bấy nhiêu trường.
        // Tách class riêng để sau này nếu muốn khóa không cho sửa trường nào (VD: Tọa độ) thì tách ra dễ dàng.
    }

    // DTO Kiểm duyệt / Khóa quán (Dành cho Admin duyệt bài, hoặc Owner tự xin tạm ngưng)
    public class SuspendEateryRequestDto
    {
        // True = Đã duyệt/Đang hoạt động | False = Khóa/Chờ duyệt
        public bool IsApproved { get; set; } 
    }

    // ==========================================
    // 2. NHÓM RESPONSE (Trả dữ liệu về)
    // ==========================================

    // DTO Trả về Cơ bản (Dành cho Tourist - Hùng xem)
    // Đã lược bỏ OwnerId, CategoryId và trạng thái kiểm duyệt để bảo mật và nhẹ app
    public class EateryTouristResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? PriceRange { get; set; }
        public string? Description { get; set; }
        public double Rating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? AudioFilePath { get; set; }
        public string? ImagePath { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
    }

    // DTO Trả về Chi tiết (Dành cho Admin - Bạn và Owner - Minh xem)
    // Kế thừa lại của Tourist và nhét thêm các trường quản lý hệ thống
    public class EateryAdminOwnerResponseDto : EateryTouristResponseDto
    {
        public int OwnerId { get; set; }
        public int CategoryId { get; set; }
        public bool IsApproved { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }
}