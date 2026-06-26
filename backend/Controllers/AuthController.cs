using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Repository;
using backend.DTOs.AuthDTO;
using System;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")] // Tự động thành: /api/v1/auth
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _authRepo;

        public AuthController(AuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        // ==========================================
        // 1. API ĐĂNG KÝ: POST /api/v1/auth/register
        // ==========================================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var newUserId = await _authRepo.RegisterAsync(request);
                return Ok(new { message = "Đăng ký tài khoản thành công!", userId = newUserId });
            }
            catch (Exception ex)
            {
                // Thường xảy ra lỗi khi username hoặc email đã tồn tại (UNIQUE constraint)
                return BadRequest(new { message = "Lỗi khi đăng ký: " + ex.Message });
            }
        }

        // ==========================================
        // 2. API ĐĂNG NHẬP: POST /api/v1/auth/login
        // ==========================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Gọi hàm AuthenticateAsync để kiểm tra thông tin và tạo token JWT thật
            var authResponse = await _authRepo.AuthenticateAsync(request.Username, request.Password);

            if (authResponse == null)
            {
                return Unauthorized(new { message = "Sai tên đăng nhập, mật khẩu hoặc tài khoản đã bị khóa!" });
            }

            // Trả về token thật và thông tin người dùng
            return Ok(authResponse);
        }

        // ==========================================
        // 3. API LẤY DANH SÁCH NGƯỜI DÙNG: GET /api/v1/auth/users
        // ==========================================
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authRepo.GetAllUsersAsync();
            return Ok(users);
        }
    }
}