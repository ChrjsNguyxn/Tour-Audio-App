using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Repository;
using backend.DTOs.EateryDTO;
using Backend.DTOs.CategoryDTO;

namespace backend.Controllers
{

    [ApiController]
    [Route("api/v1/owner")]
    public class OwnerEateryController : ControllerBase
    {
        private readonly EateryRepository _eateryRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly OwnerStatsRepository _statsRepo;

        public OwnerEateryController(
            EateryRepository eateryRepo,
            CategoryRepository categoryRepo,
            OwnerStatsRepository statsRepo)
        {
            _eateryRepo = eateryRepo;
            _categoryRepo = categoryRepo;
            _statsRepo = statsRepo;
        }

        // API: GET /api/v1/owner/categories
        // Dùng lại CategoryRepository.GetAllCategoriesAsync() đã có, không viết lại
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepo.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // API: GET /api/v1/owner/{ownerId}/eateries
        // Dùng lại EateryRepository.GetAllForAdminAsync() đã có, rồi lọc theo ownerId
        // (không thêm method mới vào EateryRepository.cs để không đụng file gốc)
        [HttpGet("{ownerId}/eateries")]
        public async Task<IActionResult> GetEateriesByOwner(int ownerId)
        {
            var all = await _eateryRepo.GetAllForAdminAsync();
            var mine = all.Where(e => e.OwnerId == ownerId);
            return Ok(mine);
        }

        // API: POST /api/v1/owner/eateries
        // Dùng lại EateryRepository.CreateEateryAsync() đã có — quán mới luôn is_approved = 0 (chờ Admin duyệt)
        [HttpPost("eateries")]
        public async Task<IActionResult> CreateEatery([FromBody] OwnerCreateEateryRequestDto request)
        {
            var newId = await _eateryRepo.CreateEateryAsync(request.OwnerId, request);
            return Ok(new { message = "Tạo quán ăn thành công, đang chờ Admin duyệt!", id = newId });
        }

        // API: PUT /api/v1/owner/eateries/{id}
        // Owner sửa quán của mình. Vì EateryRepository chưa có sẵn hàm Update dùng chung
        // cho Owner (chỉ có cho Admin qua approve), ta thêm action riêng tại đây dùng
        // trực tiếp Dapper, KHÔNG đụng EateryRepository.cs gốc.
        [HttpPut("eateries/{id}")]
        public async Task<IActionResult> UpdateEatery(int id, [FromBody] OwnerCreateEateryRequestDto request)
        {
            var success = await _statsRepo.UpdateOwnerEateryAsync(id, request);
            if (!success) return NotFound(new { message = "Không tìm thấy quán để cập nhật!" });
            return Ok(new { message = "Cập nhật quán thành công! Vui lòng chờ Admin duyệt lại nếu cần." });
        }

        // API: DELETE /api/v1/owner/eateries/{id}
        [HttpDelete("eateries/{id}")]
        public async Task<IActionResult> DeleteEatery(int id)
        {
            var success = await _statsRepo.DeleteOwnerEateryAsync(id);
            if (!success) return NotFound(new { message = "Không tìm thấy quán để xóa!" });
            return Ok(new { message = "Đã xóa quán." });
        }

        // API: GET /api/v1/owner/eateries/{id}/stats
        [HttpGet("eateries/{id}/stats")]
        public async Task<IActionResult> GetStats(int id)
        {
            var stats = await _statsRepo.GetStatsForEateryAsync(id);
            return Ok(stats);
        }
    }
}
