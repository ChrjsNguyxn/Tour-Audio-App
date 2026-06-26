namespace backend.DTOs.UserDTO
{
    // Khuôn mẫu khi Admin tạo tài khoản mới
    public class CreateUserAdminDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; } = "Tourist";
    }

    // Khuôn mẫu khi Admin sửa thông tin (không sửa password và username)
    public class UpdateUserAdminDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; } = "Tourist";
    }

    // Khuôn mẫu khi Bấm nút Khóa/Mở khóa
    public class ToggleStatusDto
    {
        public bool IsActive { get; set; }
    }
}