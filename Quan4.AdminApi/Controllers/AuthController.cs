using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quan4.AdminApi.Data;
using Quan4.AdminApi.DTOs;
using Quan4.AdminApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quan4.AdminApi.Controllers
{
    [Route("api/v1/admin/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // 1. API ĐĂNG KÝ
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("Tên đăng nhập này đã được sử dụng!");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = "super_admin" 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Đăng ký tài khoản Admin thành công!");
        }

        // 2. API ĐĂNG NHẬP
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // --- BẬT MÁY X-QUANG KIỂM TRA DỮ LIỆU ĐẦU VÀO ---
            Console.WriteLine($"\n[DEBUG] 1. React gửi qua: Username = '{request.Username}' | Password = '{request.Password}'");

            // 1. Tìm user trong DB
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            
            if (user == null) 
            {
                Console.WriteLine("[DEBUG] 2. KẾT QUẢ: Không tìm thấy Username này trong Database!");
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu!");
            }

            // --- KIỂM TRA ĐỘ DÀI MẬT KHẨU BĂM ---
            Console.WriteLine($"[DEBUG] 2. Tìm thấy User! Chuỗi Hash trong DB là: '{user.PasswordHash}'");
            Console.WriteLine($"[DEBUG] 3. Độ dài chuỗi Hash: {user.PasswordHash.Length} ký tự (Nếu dưới 60 là bị lỗi Database)");

            // 2. So sánh mật khẩu
            bool isMatch = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            Console.WriteLine($"[DEBUG] 4. KẾT QUẢ BCrypt Đối chiếu: {isMatch}\n");

            if (!isMatch)
            {
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu!");
            }

            // 3. Nếu đúng, tạo JWT Token
            var token = CreateToken(user);
            
            return Ok(new { token = token, message = "Đăng nhập thành công!" });
        }

        // 3. HÀM TẠO TOKEN
        private string CreateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("JwtSettings:SecretKey").Value!));

            // FIXED: Đã đổi từ HmacSha512Signature sang HmacSha256Signature để không bị lỗi độ dài Key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Bổ sung ID để định danh chuẩn
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}