using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.DTOs.ReviewDTO;
using backend.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewRepository _reviewRepo;

        public ReviewController(ReviewRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        // API: GET /api/v1/review/eatery/{eateryId}
        // Mô tả: Ai cũng xem được review
        [HttpGet("eatery/{eateryId}")]
        public async Task<IActionResult> GetReviews(int eateryId)
        {
            var reviews = await _reviewRepo.GetReviewsByEateryIdAsync(eateryId);
            return Ok(reviews);
        }

        // API: POST /api/v1/review/eatery/{eateryId}
        // Mô tả: Phải đăng nhập (có Token) mới được viết review
        [HttpPost("eatery/{eateryId}")]
        [Authorize] 
        public async Task<IActionResult> CreateReview(int eateryId, [FromBody] CreateReviewRequestDto request)
        {
            // Kiểm tra xem sao (Rating) có hợp lệ không (từ 1 đến 5)
            if (request.Rating < 1 || request.Rating > 5)
            {
                return BadRequest(new { message = "Số sao đánh giá phải từ 1 đến 5!" });
            }

            // Móc UserId từ Token
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            int userId = int.Parse(userIdString);

            var newId = await _reviewRepo.CreateReviewAsync(eateryId, userId, request);
            return Ok(new { message = "Cảm ơn bạn đã đánh giá!", id = newId });
        }
    }
}