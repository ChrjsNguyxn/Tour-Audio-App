using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using backend.Repository;
using Backend.DTOs.UserDTO;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepo;

        public UserController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // ==========================================
        // API: GET /api/v1/user/admin-all
        // Mô tả: Trả về danh sách chi tiết tất cả người dùng trong hệ thống
        // ==========================================
        [HttpGet("admin-all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsersForAdminAsync();
            return Ok(users);
        }

        // ==========================================
        // API: PUT /api/v1/user/{id}/suspend
        // Mô tả: Admin dùng để Khóa hoặc Mở khóa tài khoản (Truyền { "isActive": false })
        // ==========================================
        [HttpPut("{id}/suspend")]
        public async Task<IActionResult> SuspendUser(int id, [FromBody] SuspendUserRequestDto request)
        {
            var success = await _userRepo.ChangeUserStatusAsync(id, request.IsActive);
            
            if (!success)
            {
                return NotFound(new { message = "Không tìm thấy người dùng này!" });
            }

            var statusStr = request.IsActive ? "Mở khóa (Active)" : "Đã khóa (Suspend)";
            return Ok(new { message = $"Thao tác thành công: {statusStr} tài khoản!" });
        }
    }
}