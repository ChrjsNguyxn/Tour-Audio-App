using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Repository;
using backend.DTOs.UserDTO;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // Bắt buộc phải có Token JWT mới được gọi các API này
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepo;

        public UserController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // Lấy danh sách (GET /api/v1/user/admin-all)
        [HttpGet("admin-all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return Ok(users);
        }

        // Thêm mới (POST /api/v1/user)
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserAdminDto request)
        {
            var newId = await _userRepo.CreateUserAsync(request);
            return Ok(new { message = "Thêm người dùng thành công!", id = newId });
        }

        // Sửa thông tin (PUT /api/v1/user/{id})
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserAdminDto request)
        {
            await _userRepo.UpdateUserAsync(id, request);
            return Ok(new { message = "Cập nhật thông tin thành công!" });
        }

        // Khóa / Mở khóa (PUT /api/v1/user/{id}/toggle-status)
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id, [FromBody] ToggleStatusDto request)
        {
            await _userRepo.ToggleStatusAsync(id, request.IsActive);
            return Ok(new { message = "Cập nhật trạng thái thành công!" });
        }

        // Xóa (DELETE /api/v1/user/{id})
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepo.DeleteUserAsync(id);
            return Ok(new { message = "Xóa người dùng thành công!" });
        }
    }
}