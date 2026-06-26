using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Repository;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using backend.DTOs;
using Backend.DTOs.EateryDTO;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EateryController : ControllerBase
    {
        private readonly EateryRepository _eateryRepo;

        public EateryController(EateryRepository eateryRepo)
        {
            _eateryRepo = eateryRepo;
        }

        // 1. Lấy tất cả danh sách quán cho trang quản trị Admin
        [HttpGet("admin-all")]
        [Authorize]
        public async Task<IActionResult> GetAdminAll()
        {
            var eateries = await _eateryRepo.GetAllForAdminAsync();
            return Ok(eateries);
        }

        // 2. API Đặc Quyền: Đổi trạng thái quán ăn (Duyệt / Khóa / Xóa mềm) kèm lý do
        [HttpPut("{id}/change-status")]
        [Authorize]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeStatusRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Status))
            {
                return BadRequest(new { message = "Trạng thái không được để trống!" });
            }

            var result = await _eateryRepo.ChangeEateryStatusAsync(id, request.Status, request.Reason);
            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy quán ăn yêu cầu!" });
            }

            return Ok(new { message = $"Cập nhật trạng thái quán sang [{request.Status}] thành công!" });
        }

        // 3. Admin tự tạo quán ăn mới
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEatery([FromBody] CreateEateryRequestDto request)
        {
            // Lấy ID người dùng đang login từ Token JWT để làm OwnerId (Mặc định Admin tạo thì Admin sở hữu hoặc gán cho id = 1)
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int ownerId = string.IsNullOrEmpty(userIdStr) ? 1 : int.Parse(userIdStr);

            var newId = await _eateryRepo.CreateEateryAsync(ownerId, request);
            return Ok(new { message = "Admin thêm quán ăn thành công!", id = newId });
        }

        // 4. Admin chỉnh sửa thông tin cơ bản của quán
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEatery(int id, [FromBody] UpdateEateryAdminDto request)
        {
            await _eateryRepo.UpdateEateryAsync(id, request);
            return Ok(new { message = "Cập nhật thông tin quán thành công!" });
        }
    }
}