using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.DTOs.EateryDTO;
using backend.Repository;

namespace backend.Controllers
{
    // BẮT BUỘC CÓ 2 DÒNG NÀY ĐỂ ĐỊNH TUYẾN API
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EateryController : ControllerBase
    {
        private readonly EateryRepository _eateryRepo;

        public EateryController(EateryRepository eateryRepo)
        {
            _eateryRepo = eateryRepo;
        }

        // ==========================================
        // API: GET /api/v1/eatery/admin-all
        // Mô tả: Lấy danh sách tất cả quán ăn (bao gồm cả quán chưa duyệt) để Admin quản lý
        // ==========================================
        [HttpGet("admin-all")]
        public async Task<IActionResult> GetAllEateriesForAdmin()
        {
            var eateries = await _eateryRepo.GetAllForAdminAsync();
            return Ok(eateries);
        }

        // ==========================================
        // API: PUT /api/v1/eatery/{id}/approve
        // Mô tả: Admin duyệt hoặc khóa quán ăn (truyền lên { "isApproved": true/false })
        // ==========================================
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveEatery(int id, [FromBody] SuspendEateryRequestDto request)
        {
            var success = await _eateryRepo.ChangeApprovalStatusAsync(id, request.IsApproved);
            
            if (!success) 
            {
                return NotFound(new { message = "Không tìm thấy quán ăn này!" });
            }
            
            var statusStr = request.IsApproved ? "Đã duyệt" : "Đã khóa/Hủy duyệt";
            return Ok(new { message = $"Thao tác thành công: {statusStr} quán ăn!" });
        }

        // ==========================================
        // API: POST /api/v1/eatery
        // Mô tả: Tạo quán ăn mới (Dành cho Admin hoặc Owner)
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> CreateEatery([FromBody] CreateEateryRequestDto request)
        {
            // Tạm thời gán cứng OwnerId = 1 để test.
            // Sau này làm chức năng Đăng nhập xong, ta sẽ trích xuất ID từ Token của người dùng.
            int dummyOwnerId = 1; 
            
            var newId = await _eateryRepo.CreateEateryAsync(dummyOwnerId, request);
            
            return CreatedAtAction(
                nameof(GetAllEateriesForAdmin), 
                new { id = newId }, 
                new { message = "Tạo quán ăn thành công, đang chờ duyệt!", id = newId }
            );
        }
    }
}