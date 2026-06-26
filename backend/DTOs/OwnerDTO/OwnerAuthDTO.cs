namespace backend.DTOs.OwnerDTO
{
    // Request đăng nhập Owner — dùng chung bảng users, chỉ chặn role
    public class OwnerLoginRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Request đăng ký Owner — gắn cứng role = "owner" ở Controller, không cho client tự gửi role
    public class OwnerRegisterRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
