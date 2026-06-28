using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.Extensions.Configuration;
using backend.DTOs.EateryDTO;
using backend.DTOs.StatsDTO;

namespace backend.Repository
{

    public class OwnerStatsRepository
    {
        private readonly string _connectionString;

        public OwnerStatsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=Database/foodtour.db";
        }

        // ---------- Update / Delete quán của Owner ----------

        public async Task<bool> UpdateOwnerEateryAsync
        (int id, int ownerId, OwnerCreateEateryRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                UPDATE eateries SET
                    name = @Name,
                    address = @Address,
                    category_id = @CategoryId,
                    price_range = @PriceRange,
                    description = @Description,
                    latitude = @Latitude,
                    longitude = @Longitude,
                    audio_file_path = @AudioFilePath,
                    image_path = @ImagePath,
                    open_time = @OpenTime,
                    close_time = @CloseTime,
                    is_open_now = @IsOpenNow,
                    narration_text = @NarrationText,
                    narration_language = @NarrationLanguage
                WHERE id = @Id AND owner_id = @OwnerId";

            var affected = await connection.ExecuteAsync(sql, new
            {
                Id = id,
                request.Name,
                request.Address,
                request.CategoryId,
                request.PriceRange,
                request.Description,
                request.Latitude,
                request.Longitude,
                request.AudioFilePath,
                request.ImagePath,
                request.OpenTime,
                request.CloseTime,
                request.IsOpenNow,
                request.NarrationText,
                request.NarrationLanguage,
                OwnerId = ownerId
            });

            return affected > 0;
        }

        public async Task<bool> DeleteOwnerEateryAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            // Xóa stats + menu items liên quan trước để không vướng dữ liệu rác
            await connection.ExecuteAsync("DELETE FROM eatery_stats WHERE eatery_id = @Id", new { Id = id });
            await connection.ExecuteAsync("DELETE FROM menu_items WHERE eatery_id = @Id", new { Id = id });
            var affected = await connection.ExecuteAsync("DELETE FROM eateries WHERE id = @Id", new { Id = id });
            return affected > 0;
        }

        // ---------- Thống kê (bảng eatery_stats MỚI) ----------

        public async Task<EateryStatsResponseDto> GetStatsForEateryAsync(int eateryId)
        {
            using var connection = new SqliteConnection(_connectionString);

            var totals = await connection.QuerySingleAsync<(int views, int listens, int favorites)>(@"
                SELECT
                    COALESCE(SUM(CASE WHEN event_type = 'view' THEN 1 ELSE 0 END), 0) AS views,
                    COALESCE(SUM(CASE WHEN event_type = 'listen' THEN 1 ELSE 0 END), 0) AS listens,
                    COALESCE(SUM(CASE WHEN event_type = 'favorite' THEN 1 ELSE 0 END), 0) AS favorites
                FROM eatery_stats
                WHERE eatery_id = @EateryId",
                new { EateryId = eateryId });

            var dailyRows = await connection.QueryAsync<(string day, int count)>(@"
                SELECT strftime('%d/%m', created_at) AS day, COUNT(*) AS count
                FROM eatery_stats
                WHERE eatery_id = @EateryId
                  AND event_type = 'view'
                  AND created_at >= datetime('now', '-7 days')
                GROUP BY day
                ORDER BY created_at ASC",
                new { EateryId = eateryId });

            return new EateryStatsResponseDto
            {
                TotalViews = totals.views,
                TotalListens = totals.listens,
                TotalFavorites = totals.favorites,
                Last7Days = dailyRows.Select(r => new DailyStatPointDto { Date = r.day, Count = r.count }).ToList()
            };
        }

        // Ghi nhận 1 lượt tương tác (view/listen/favorite) — dùng cho phía Tourist sau này
        public async Task LogEventAsync(int eateryId, string eventType)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.ExecuteAsync(
                "INSERT INTO eatery_stats (eatery_id, event_type) VALUES (@EateryId, @EventType)",
                new { EateryId = eateryId, EventType = eventType });
        }
    }
}