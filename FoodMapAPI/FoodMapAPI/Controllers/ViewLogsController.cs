using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodMapAPI.Data;
using FoodMapAPI.Models;

namespace FoodMapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViewLogsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ViewLogsController(AppDbContext context) { _context = context; }

        // POST: api/viewlogs  -- ghi log khi User xem/nghe quán
        [HttpPost]
        public async Task<IActionResult> LogAction([FromBody] ViewLog log)
        {
            log.CreatedAt = DateTime.Now;
            _context.ViewLogs.Add(log);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: api/viewlogs/shop/5 -- Owner xem thống kê quán mình
        [HttpGet("shop/{shopId}")]
        public async Task<ActionResult> GetStatsByShop(int shopId)
        {
            var logs = await _context.ViewLogs.Where(l => l.ShopId == shopId).ToListAsync();

            var totalViews = logs.Count(l => l.ActionType == "view");
            var totalListens = logs.Count(l => l.ActionType == "listen_audio");
            var totalFavorites = logs.Count(l => l.ActionType == "favorite");

            var last7Days = logs
                .Where(l => l.CreatedAt >= DateTime.Now.AddDays(-7))
                .GroupBy(l => l.CreatedAt.Date)
                .Select(g => new { date = g.Key.ToString("dd/MM"), count = g.Count() })
                .OrderBy(g => g.date)
                .ToList();

            return Ok(new { totalViews, totalListens, totalFavorites, last7Days });
        }
    }
}