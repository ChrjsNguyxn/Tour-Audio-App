using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.DTOs.TouristDTO;
using backend.Repository;
<<<<<<< HEAD
=======
using backend.Services;
>>>>>>> 9ac07cc6df21a2ba074a0e0cd1990a3cc86e9517

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TouristController : ControllerBase
    {
<<<<<<< HEAD
        private readonly TouristRepository _touristRepo;

        public TouristController(TouristRepository touristRepo)
        {
            _touristRepo = touristRepo;
=======
        private readonly TouristService _touristService;

        public TouristController(TouristService touristService)
        {
            _touristService = touristService;
>>>>>>> 9ac07cc6df21a2ba074a0e0cd1990a3cc86e9517
        }

        // API: GET /api/v1/tourist/eateries
        // Mô tả: Lấy danh sách quán ăn đã duyệt (Dành cho trang chủ)
        [HttpGet("eateries")]
        public async Task<IActionResult> GetApprovedEateries()
        {
<<<<<<< HEAD
            var eateries = await _touristRepo.GetApprovedEateriesAsync();
=======
            var eateries = await _touristService.GetAllPOIsAsync();
>>>>>>> 9ac07cc6df21a2ba074a0e0cd1990a3cc86e9517
            return Ok(eateries);
        }

        // API: GET /api/v1/tourist/eatery/{id}
<<<<<<< HEAD
        // Mô tả: Lấy chi tiết 1 quán ăn
        [HttpGet("eatery/{id}")]
        public async Task<IActionResult> GetEateryDetail(int id)
        {
            var eatery = await _touristRepo.GetEateryDetailAsync(id);
            if (eatery == null)
            {
                return NotFound(new { message = "Không tìm thấy quán ăn, hoặc quán chưa được duyệt!" });
            }
            return Ok(eatery);
=======
        // Mô tả: Lấy chi tiết 1 quán ăn theo id
        [HttpGet("eatery/{id}")]
        public async Task<IActionResult> GetEateryDetail(int id)
        {
            var poi = _touristService.GetPOIbyID(id);

            if(poi == null) return NotFound();

            return Ok(poi);
>>>>>>> 9ac07cc6df21a2ba074a0e0cd1990a3cc86e9517
        }
    }
}