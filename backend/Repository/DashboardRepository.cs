using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using backend.DTOs.DashboardDTO;
using Microsoft.Extensions.Configuration;

namespace backend.Repository
{
    public class DashboardRepository
    {
        private readonly string _connectionString;

        public DashboardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Database/foodtour.db";
        }

        public async Task<DashboardResponseDto> GetDashboardStatsAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            
            // Gom 5 câu đếm (COUNT) vào 1 lệnh truy vấn duy nhất
            var sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM users) AS TotalUsers,
                    (SELECT COUNT(*) FROM eateries) AS TotalEateries,
                    (SELECT COUNT(*) FROM eateries WHERE is_approved = 0) AS TotalPendingEateries,
                    (SELECT COUNT(*) FROM menu_items) AS TotalMenuItems,
                    (SELECT COUNT(*) FROM reviews) AS TotalReviews;";
            
            return await connection.QuerySingleAsync<DashboardResponseDto>(sql);
        }
    }
}