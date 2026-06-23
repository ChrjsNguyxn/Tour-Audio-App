using System;

namespace Backend.DTOs.CategoryDTO
{
    // ==========================================
    // 1. NHÓM REQUEST (Admin gửi dữ liệu lên)
    // ==========================================

    // DTO Tạo mới Danh mục (Chỉ Admin)
    public class CreateCategoryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    // DTO Cập nhật Danh mục (Chỉ Admin)
    public class UpdateCategoryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    // ==========================================
    // 2. NHÓM RESPONSE (Trả dữ liệu về)
    // ==========================================

    // DTO Trả về (Dùng chung cho Admin quản lý và Owner chọn khi tạo quán)
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }
}