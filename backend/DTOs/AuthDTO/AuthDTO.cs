namespace backend.DTOs.AuthDTO
{
    // Khuôn mẫu dữ liệu User gửi lên khi đăng nhập
    public class LoginRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Khuôn mẫu dữ liệu trả về (Chứa Token quyền lực)
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}