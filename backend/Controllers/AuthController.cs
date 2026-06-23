using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.DTOs.AuthDTO;
using backend.Repository;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _authRepo;

        public AuthController(AuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        // API: POST /api/v1/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authRepo.AuthenticateAsync(request.Username, request.Password);

            if (result == null)
            {
                return Unauthorized(new { message = "Sai tên đăng nhập, mật khẩu hoặc tài khoản đã bị khóa!" });
            }

            return Ok(new { message = "Đăng nhập thành công!", data = result });
        }
    }
}