using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Repository;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // Bắt buộc phải có Token đăng nhập
    public class DashboardController : ControllerBase
    {
        private readonly DashboardRepository _dashboardRepo;

        public DashboardController(DashboardRepository dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
        }

        // API: GET /api/v1/dashboard/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _dashboardRepo.GetDashboardStatsAsync();
            return Ok(stats);
        }
    }
}