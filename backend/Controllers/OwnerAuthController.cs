using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.DTOs.OwnerDTO;
using backend.Repository;
using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{

    [ApiController]
    [Route("api/v1/owner-auth")]
    public class OwnerAuthController : ControllerBase
    {
        private readonly AuthRepository _authRepo;
        private readonly string _connectionString;

        public OwnerAuthController(AuthRepository authRepo, IConfiguration configuration)
        {
            _authRepo = authRepo;
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=Database/foodtour.db";
        }

        // API: POST /api/v1/owner-auth/login
        // Dùng lại đúng AuthRepository.AuthenticateAsync() đã có (KHÔNG viết lại logic JWT)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] OwnerLoginRequestDto request)
        {
            var result = await _authRepo.AuthenticateAsync(request.Username, request.Password);

            if (result == null)
            {
                return Unauthorized(new { message = "Sai tên đăng nhập, mật khẩu hoặc tài khoản đã bị khóa!" });
            }

            // Chặn nếu tài khoản không phải Owner (vd: tourist cố đăng nhập vào trang owner)
            if (result.Role != "owner")
            {
                return Unauthorized(new { message = "Tài khoản này không có quyền truy cập trang chủ quán!" });
            }

            return Ok(new { message = "Đăng nhập thành công!", data = result });
        }

        // API: POST /api/v1/owner-auth/register
        // Đăng ký tài khoản Owner mới — gắn cứng role = "owner", không cho client tự gửi role
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] OwnerRegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!" });
            }

            using var connection = new SqliteConnection(_connectionString);

            var existed = await connection.QuerySingleOrDefaultAsync<int?>(
                "SELECT id FROM users WHERE username = @Username",
                new { request.Username });

            if (existed != null)
            {
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại!" });
            }

            var sql = @"
                INSERT INTO users (username, password, role, full_name, email, is_active)
                VALUES (@Username, @Password, 'owner', @FullName, @Email, 1);
                SELECT last_insert_rowid();";

            var newId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                request.Username,
                request.Password,
                request.FullName,
                request.Email
            });

            return Ok(new { message = "Đăng ký chủ quán thành công! Hãy đăng nhập.", id = newId });
        }
    }
}
