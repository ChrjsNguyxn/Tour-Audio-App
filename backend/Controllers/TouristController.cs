using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.DTOs.TouristDTO;
using backend.Repository;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TouristController : ControllerBase
    {
        private readonly TouristRepository _touristRepo;
        private readonly TouristService _touristService;

        public TouristController(TouristRepository touristRepo,TouristService touristService )
        {
            _touristRepo = touristRepo;
            _touristService = touristService;
        }
    

        // API: GET /api/v1/tourist/eateries
        // Mô tả: Lấy danh sách quán ăn đã duyệt (Dành cho trang chủ)
        [HttpGet("eateries")]
        public async Task<IActionResult> GetApprovedEateries()
        {

            //var eateries = await _touristRepo.GetApprovedEateriesAsync();

            var eateries = await _touristService.GetAllPOIsAsync();

            return Ok(eateries);
        }

        // API: GET /api/v1/tourist/eatery/{id}
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
        }

        
    }
}