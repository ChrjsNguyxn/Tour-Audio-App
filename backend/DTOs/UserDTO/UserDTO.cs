using System;

namespace Backend.DTOs.UserDTO
{
    // ==========================================
    // 1. NHÓM REQUEST (Nhận dữ liệu từ Client)
    // ==========================================

    // DTO Đăng ký (All)
    public class RegisterRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        // Hùng và Minh khi đăng ký sẽ truyền role ('tourist' hoặc 'owner'). 
        // Admin sẽ can thiệp logic này trong Controller để tránh hacker tự gửi role 'admin'.
        public string Role { get; set; } = "tourist"; 
    }

    // DTO Đăng nhập (All)
    public class LoginRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // DTO Khóa tài khoản (Chỉ dành cho Admin - Bạn)
    public class SuspendUserRequestDto
    {
        // Admin chỉ cần gửi true/false lên để mở/khóa
        public bool IsActive { get; set; } 
    }

    // ==========================================
    // 2. NHÓM RESPONSE (Trả dữ liệu về Client)
    // ==========================================

    // DTO Trả về Cơ bản (Dành cho Tourist - Hùng xem)
    // 💡 Lưu ý cho Hùng: Khi hiển thị ai đang đánh giá quán ăn, chỉ dùng DTO này để bảo mật Email.
    public class UserBasicResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = string.Empty;
    }

    // DTO Trả về Chi tiết (Dành cho Admin - Bạn và Owner - Minh xem)
    // Kế thừa lại toàn bộ thông tin cơ bản và thêm các trường nhạy cảm
    public class UserDetailResponseDto : UserBasicResponseDto
    {
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CreatedAt { get; set; } = string.Empty; 
    }
}